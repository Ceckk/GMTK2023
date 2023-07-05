using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ServerManager : MonoSingleton<ServerManager>
{
    private const string CUSTOM_ID = "custom_id";

    private string _guid;

    protected string _minVersion;
    protected string _playerId;

    protected DateTime _serverDate;
    protected float _timeOffsetInSeconds;

    protected object _userData;
    protected object _titleData;

    public string PlayerId { get => _playerId; }
    public object UserData { get => _userData; }
    public object TitleData { get => _titleData; }

    public abstract void Login(Action<bool, object> callback, string customDeviceId = null);

    public abstract void Logout();

    public abstract void GetTitleData(List<string> keys, Action<bool, object> callback);

    public abstract void GetUserData(List<string> keys, Action<bool, object> callback);

    public abstract void UploadUserData(object data, Action<bool> callback);

    public abstract void ResetSave(Action<bool> callback, List<string> keysToRemove = null);

    public abstract SaveData FromCloud(object data);

    public abstract object SaveDataToCloudData(SaveData saveData);

    public abstract void FecthMail(Action<bool> callback);

    public abstract void SendMail(string targetId, object data, Action<bool> callback);

    public abstract void GetCurrentServerTime(Action<bool, DateTime> callback);

    public bool CheckVersion()
    {
        if (!string.IsNullOrEmpty(_minVersion))
        {
            var currentVersion = Application.version.Split('.');
            var requiredVersion = _minVersion.Split('.');

            for (int i = 0; i < requiredVersion.Length; i++)
            {
                var requiredValue = int.Parse(requiredVersion[i]);
                var currentValue = 0;
                if (i < currentVersion.Length)
                {
                    currentValue = int.Parse(currentVersion[i]);
                }

                if (requiredValue > currentValue)
                {
                    return false;
                }
                else if (requiredValue < currentValue)
                {
                    return true;
                }
            }
        }

        Debug.LogWarning("Min version not set");
        return true;
    }

    public string GetDeviceId()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        //http://answers.unity3d.com/questions/430630/how-can-i-get-android-id-.html
        AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
        return clsSecure.CallStatic<string>("getString", objResolver, "android_id");
#elif !UNITY_EDITOR && UNITY_IPHONE
        return UnityEngine.iOS.Device.vendorIdentifier;
#else
        if (string.IsNullOrEmpty(_guid))
        {
            _guid = PlayerPrefs.GetString(CUSTOM_ID, null);
        }

        if (string.IsNullOrEmpty(_guid))
        {
            _guid = Guid.NewGuid().ToString();
            PlayerPrefs.SetString(CUSTOM_ID, _guid);
        }

        return _guid;
#endif
    }
}