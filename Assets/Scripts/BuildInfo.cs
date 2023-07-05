using System;
using UnityEngine;

[CreateAssetMenu(fileName = "_BuildInfo", menuName = "ScriptableObjects/BuildInfo", order = 1)]
public class BuildInfo : ScriptableSingleton<BuildInfo>
{
    public string buildNumber;
}
