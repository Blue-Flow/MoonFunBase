using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Base,
    Greenhouse,
    Fun,
    SolarPanel
}

public enum ResourceType
{
    Materials,
    Fun,
    Oxygen,
    Energy
}

public class Building : MonoBehaviour
{
    public BuildingType type;

    [Header("Production")]
    public bool doesProduceResource;
    public ResourceType productionResource;
    public int productionResourcePerTurn;

    [Header("Maintenance")]
    public bool hasMaintenanceCost;
    public ResourceType[] maintenanceResource;
    public int[] maintenanceResourcePerTurn;

    /*public Dictionary<ResourceType, int> maintenanceResources = new Dictionary<ResourceType, int>()
    {
        {ResourceType.Materials, 0 },
        {ResourceType.Oxygen, 0},
        {ResourceType.Energy, 0},
        {ResourceType.Fun, 0 }
    };*/
}