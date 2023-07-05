using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveManager : MonoSingleton<SaveManager>
{
    private int VERSION = 1;

    [SerializeField] private bool _cloudSaveEnabled = true;
    [SerializeField] private bool _localSaveEnabled = true;

    [SerializeField] private SaveData _currentSaveData;

    public string SavePath
    {
        get => Path.Combine(Application.persistentDataPath, "data.sav");
    }
    public SaveData CurrentSaveData { get => _currentSaveData; }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void Load()
    {
        var cloudSave = LoadCloud();
        var localSave = LoadLocal();

        if (cloudSave != null && cloudSave.version == VERSION)
        {
            _currentSaveData = cloudSave;
            return;
        }

        if (localSave != null && localSave.version == VERSION)
        {
            _currentSaveData = localSave;
            return;
        }
    }

    public void Save(Action<bool> callback)
    {
        _currentSaveData.version = VERSION;
        SaveLocal();
        SaveCloud(callback);
    }

    public void Delete()
    {
        DeleteLocal();
        DeleteCloud((success) =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    public void SaveCloud(Action<bool> callback)
    {
        if (!_cloudSaveEnabled)
        {
            callback?.Invoke(true);
            return;
        }

        var data = ServerManager.Instance.SaveDataToCloudData(_currentSaveData);
        ServerManager.Instance.UploadUserData(data, callback);
    }

    public SaveData LoadCloud()
    {
        if (!_cloudSaveEnabled)
            return null;

        var saveData = ServerManager.Instance.FromCloud(ServerManager.Instance.UserData);
        return saveData;
    }

    public void DeleteCloud(Action<bool> callback)
    {
        ServerManager.Instance.ResetSave(callback);
    }

    public void SaveLocal()
    {
        if (!_localSaveEnabled)
            return;

        string saveEncrypted = EncryptDecrypt(JsonUtility.ToJson(_currentSaveData, false));
        File.WriteAllText(SavePath, saveEncrypted);
        Debug.Log($"Saved locally at {SavePath}");
    }

    public SaveData LoadLocal()
    {
        if (!_localSaveEnabled)
            return null;

        try
        {
            return JsonUtility.FromJson<SaveData>(EncryptDecrypt(File.ReadAllText(SavePath)));
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public void DeleteLocal()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }
    }

    //XOR encryption by key, basiclly it takes ASCII code of character and ^ by key, does that to each character of string
    private string EncryptDecrypt(string textToEncrypt)
    {
        var key = 255;
        var inSb = new StringBuilder(textToEncrypt);
        var outSb = new StringBuilder(textToEncrypt.Length);
        char c;
        for (int i = 0; i < textToEncrypt.Length; i++)
        {
            c = inSb[i];
            c = (char)(c ^ key);
            outSb.Append(c);
        }
        return outSb.ToString();
    }
}
