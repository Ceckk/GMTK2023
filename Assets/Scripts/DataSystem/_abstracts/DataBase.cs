using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class DataBase : ScriptableObject
{
    public abstract void Sync();
}

public abstract class DataBase<T> : DataBase where T : Asset
{
    private const string OBSOLETE_FOLDER_NAME = "_obsolete";
    private const string OBSOLETE_PREFIX = "_obsolete_";

    protected virtual string ID { get => "_id"; }

    [SerializeField] protected TextAsset _csvTextAsset;
    [SerializeField] protected List<T> _assets = new List<T>();

    private static DataBase<T> _instance;
    public static DataBase<T> Instance
    {
        get
        {
            if (_instance == null)
            {
                string path = "_" + typeof(T).FullName.Replace("Asset", "DB");
                _instance = Resources.Load<DataBase<T>>(path);
            }

            return _instance;
        }
    }

    public ReadOnlyCollection<T> Assets { get => _assets != null ? _assets.AsReadOnly() : null; }
    public TextAsset CSVTextAsset { get => _csvTextAsset; set => _csvTextAsset = value; }

    public T GetAssetByName(string valueName)
    {
        for (int i = 0; i < _assets.Count; i++)
        {
            if (_assets[i].name == valueName)
                return _assets[i];
        }

        return null;
    }

    public T GetAssetById(string id)
    {
        for (int i = 0; i < _assets.Count; i++)
        {
            if (_assets[i].Id == id)
                return _assets[i];
        }

        return null;
    }

    [Button]
    public override void Sync()
    {
        ParseTextAsset(_csvTextAsset);
    }

    public void ParseTextAsset(TextAsset textAsset)
    {
        var backup = new List<T>(_assets);

        try
        {
            var result = TextAssetParser.ReadAsCSV(textAsset);

            // _logger.Log(string.Format("STARTED: Parsing of {0}", name));
            RemoveObsoleteAssets(result);
            UpdateOrCreateAssets(result);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
            // _logger.Log(string.Format("ENDED: Parsing of {0}", name));
        }
        catch (Exception e)
        {
            _assets = backup;
            Debug.LogException(e);
        }
    }

    protected void RemoveObsoleteAssets(List<Dictionary<string, string>> result)
    {
        var idList = new List<string>();

        foreach (var entry in result)
        {
            var id = entry[ID].ConvertToId();
            idList.Add(id);
        }

        var filteredAssets = new List<T>();

        foreach (var asset in _assets)
        {
            if (idList.Contains(asset.Id))
            {
                filteredAssets.Add(asset);
            }
            else
            {
                MoveAssetToObsoleteFolder(asset);
            }
        }

        _assets = filteredAssets;
    }

    protected void MoveAssetToObsoleteFolder(T asset)
    {
#if UNITY_EDITOR
        var assetPath = UnityEditor.AssetDatabase.GetAssetPath(asset);
        var assetFolder = GetAssetsFolder();
        var assetName = System.IO.Path.GetFileName(assetPath);
        var targetFolder = string.Format("{0}{1}{2}{1}", assetFolder, System.IO.Path.DirectorySeparatorChar, OBSOLETE_FOLDER_NAME);


        if (!System.IO.Directory.Exists(targetFolder))
        {
            UnityEditor.AssetDatabase.CreateFolder(assetFolder, OBSOLETE_FOLDER_NAME);
        }

        var result = UnityEditor.AssetDatabase.MoveAsset(assetPath, targetFolder + OBSOLETE_PREFIX + assetName);
        if (!string.IsNullOrEmpty(result))
        {
            Debug.LogError(result);
        }
        else
        {
            Debug.Log(string.Format("Moved {0} to {1} folder", assetName, OBSOLETE_FOLDER_NAME));
        }
#endif
    }

    protected abstract void UpdateOrCreateAssets(List<Dictionary<string, string>> result);

    protected string GetAssetsFolder()
    {
        return string.Format(string.Format("Assets{0}ScriptableObjects{0}{1}s", System.IO.Path.DirectorySeparatorChar, typeof(T).ToString()));
    }

    protected T CreateAsset(string assetName)
    {
        try
        {
            var asset = ScriptableObject.CreateInstance<T>();
#if UNITY_EDITOR
            if (!UnityEditor.AssetDatabase.IsValidFolder(GetAssetsFolder()))
            {
                UnityEditor.AssetDatabase.CreateFolder($"Assets{System.IO.Path.DirectorySeparatorChar}ScriptableObjects", $"{typeof(T).ToString()}s");
            }
            var assetPath = string.Format("{0}{1}{2}.asset", GetAssetsFolder(), System.IO.Path.DirectorySeparatorChar, assetName);
            UnityEditor.AssetDatabase.CreateAsset(asset, assetPath);
            Debug.Log(string.Format("Created {0} inside {1} folder", assetName, GetAssetsFolder()));
#endif
            return asset;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    [Button]
    private void GenerateIdFromName(string prefix)
    {
        for (int i = 0; i < _assets.Count; i++)
        {
            var asset = _assets[i];
            asset.Id = $"{prefix}_{asset.name}";
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(asset);
#endif
        }
    }

}
