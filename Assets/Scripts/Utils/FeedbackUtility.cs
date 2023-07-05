using System;
using UnityEngine;
using UnityEngine.Networking;

public static class FeedbackUtility
{
    public static void SendEmail()
    {
        string email = "support@finifugu.games";
        string subject = CustomEscapeURL("Support For " + Application.productName);
        var utcTime = DateTime.UtcNow.ToLocalTime();
        var game = Application.productName;
        var version = string.Format("{0}.{1}", Application.version, BuildInfo.Instance.buildNumber);
        var deviceId = SystemInfo.deviceUniqueIdentifier;
        var playerId = ServerManager.Instance != null ? ServerManager.Instance.PlayerId : "";
        var device = SystemInfo.deviceModel;

        string body = CustomEscapeURL(
            "--------DO NOT DELETE!--------\n"
            + " \n"
            + "UTC Time:" + utcTime + "\n"
            + "Game:" + game + "\n"
            + "Version:" + version + "\n"
            + "Device ID:" + deviceId + "\n"
            + "Player ID:" + playerId + "\n"
            + device + "\n"
            + " \n"
            + "--------DO NOT DELETE!--------\n"
            + "Dear Finifugu, "
            );
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    private static string CustomEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
}
