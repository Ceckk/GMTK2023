using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Asset : ScriptableObject
{
    [SerializeField, ReadOnly] private string _id;
    public string Id { get => _id; set => _id = value; }
}
