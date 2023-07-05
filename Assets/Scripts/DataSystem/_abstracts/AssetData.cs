using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class AssetData<T> where T : Asset
{
    [SerializeField, ReadOnly] protected string _id;

    public AssetData(T asset)
    {
        _id = asset.Id;
    }

    public string Id { get => _id; }
    
    public T Asset { get => DataBase<T>.Instance.GetAssetById(_id); }
}
