using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "TilePreset", menuName = "ScriptableObjects/Tiles")]
public class TilePreset : ScriptableObject
{
    public bool hasBonus;
    public bool hasMalus;
    public ResourceType resourceType;
    public int modificationToApply;
}
