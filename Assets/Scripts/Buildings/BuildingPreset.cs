using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu (fileName = "BuildingPrefab", menuName = "ScriptableObjects/Buildings")]
public class BuildingPreset : ScriptableObject
{
    public bool doesProduceResource;
    public ResourceType productionResource;
    public int productionResourcePerTurn;

    [Header("Maintenance")]
    public bool hasMaintenanceCost;
    public ResourceType[] maintenanceResource;
    public int[] maintenanceResourcePerTurn;
}
