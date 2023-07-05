var LOCK_KEY = "lock";
var LOCK_EXPIRY_TIME_SECONDS = 300;

var MAIL_KEY = "mail";

function acquireLock(targetPlayfabId, currentPlayfabId)
{
    // Check if lock object exists in target player's user data
    var userDataResult = server.GetUserInternalData({ PlayFabId: targetPlayfabId, Keys: [LOCK_KEY] });
    var lockData = userDataResult.Data[LOCK_KEY];
    if (lockData !== undefined)
    {
        // Lock object exists, check if it's currently being used
        var lockObject = JSON.parse(lockData.Value);
        var dateNow = Date.now();
        if (dateNow - lockObject.expiryTime < LOCK_EXPIRY_TIME_SECONDS)
        {
            if (lockObject.lockedBy !== currentPlayfabId)
            {
                return { success: false, errorCode: "LockAlreadyHeld", errorDetails: "Lock is already in use" };
            }
        }
    }

    var lockObject = { lockedBy: currentPlayfabId, expiryTime: dateNow + (LOCK_EXPIRY_TIME_SECONDS * 1000) };
    var lockValue = JSON.stringify(lockObject);
    server.UpdateUserInternalData({ PlayFabId: targetPlayfabId, Data: { [LOCK_KEY]: lockValue } });
    return { success: true };
};

function releaseLock(targetPlayfabId)
{
    server.UpdateUserInternalData({ PlayFabId: targetPlayfabId, Data: { [LOCK_KEY]: null } });
};

handlers.sendMail = function (args, context)
{
    var targetPlayfabId = args.playfabId;
    var currentPlafabId = context.playerProfile.PlayerId;

    // Try to acquire target lock
    var lockResult = acquireLock(targetPlayfabId, currentPlafabId);
    if (!lockResult.success)
    {
        return lockResult;
    }

    var data = args.data;

    // TODO: check that the user has the item that he want to send if any

    // Send Mail
    var mailResult = server.GetUserData(
        {
            PlayFabId: targetPlayfabId,
            Keys: [MAIL_KEY]
        }
    );

    var mail = mailResult.Data[MAIL_KEY] ? JSON.parse(mailResult.Data[MAIL_KEY].Value) : [];

    mail.push(
        {
            from: currentPlafabId,
            data: data,
            read: false,
            collected: false,
            timestamp: getCurrentTime(),
        }
    );

    // TODO: remove item from current user

    var uploadUserMailRequest = {
        PlayFabId: targetPlayfabId,
        Data: {
            [MAIL_KEY]: mail,
        },
        Permission: "Public"
    };

    server.UpdateUserData(uploadUserMailRequest);

    releaseLock(targetPlayfabId);

    return { success: true };
};

handlers.getCurrentTime = function ()
{
    var now = new Date();
    return now.getTime();
};