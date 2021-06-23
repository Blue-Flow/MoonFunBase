using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentTurn;
    public int maxFun = 10;

    [Header("Current Resources")]
    private int currentMaterials = 1;

    [Header("Round Resource Increase")]
    private int funPerTurn;
    private int oxygenPerTurn;
    private int energyPerTurn;

    private int funCap1 = 5;
    private int funCap2 = 10;
    private bool funCap1Reached = false;

    private void Awake ()
    {
        EventsSubscribe();
    }
    private void Start()
    {
        EventHandler.StartGame();
    }
    private void GenerateValues()
    {
        UI.instance.SetMaxFun(maxFun);

        // Update the values on the UI
        UI.instance.UpdateTurnText(currentTurn);
        EventHandler.ValueChanged(ResourceType.Fun, funPerTurn);
        EventHandler.ValueChanged(ResourceType.Oxygen, oxygenPerTurn);
        EventHandler.ValueChanged(ResourceType.Energy, energyPerTurn);
        EventHandler.ValueChanged(ResourceType.Materials, currentMaterials);

    }
    private void CheckBuildingConditions(BuildingPreset buildingPreset)
    {
        if (currentMaterials >= 1) // TODO : (currentMaterials >= buildingConstructionCost)
        {
            EventHandler.BuildStarted(buildingPreset);
        }
        else
        {
            // Display a notification screen with the appropriate error : not enough materials to build
            EventHandler.Error(0);
        }
    }
    private void EndTurn ()
    {
        currentMaterials ++;
        EventHandler.ValueChanged(ResourceType.Materials, currentMaterials);

        CheckEndGame();

        currentTurn++;
        UI.instance.UpdateTurnText(currentTurn);
    }

    #region Resources values
    // called when a new building has been created and placed down
    private void ModifyValues_Building(BuildingPreset buildingPreset, TileType arg2, Vector2 arg3)
    {
        // resource the building produces
        if (buildingPreset.doesProduceResource)
        {
            // Add the resources to give to the count
            AddBuildingProduction(buildingPreset);
        }
        // resource the building may cost
        if (buildingPreset.hasMaintenanceCost)
        {
            // Substract the resources to take from the count
            AddBuildingMaintenance(buildingPreset);
        }
        currentMaterials -= 1;
        EventHandler.ValueChanged(ResourceType.Materials, currentMaterials);
        EventHandler.BuildOver();
    }
    private void ModifyValues_Tile(BuildingPreset buildingPreset, TileType tileType, Vector2 position)
    {
        if (tileType != TileType.Neutral)
        {
            switch (tileType)
            {
                case TileType.MinusDioxygen:
                    if (buildingPreset.buildingType == BuildingType.Oxygen)
                    {
                        oxygenPerTurn--;
                        EventHandler.ValueChanged(ResourceType.Oxygen, oxygenPerTurn);
                    }
                    break;
                case TileType.PlusDioxygen:
                    if (buildingPreset.buildingType == BuildingType.Oxygen)
                    {
                        oxygenPerTurn++;
                        EventHandler.ValueChanged(ResourceType.Oxygen, oxygenPerTurn);
                    }
                    break;
                case TileType.MinusEnergy:
                    if (buildingPreset.buildingType == BuildingType.Energy)
                    {
                        energyPerTurn--;
                        EventHandler.ValueChanged(ResourceType.Energy, energyPerTurn);
                    }
                    break;
                case TileType.PlusEnergy:
                    if (buildingPreset.buildingType == BuildingType.Energy)
                    {
                        energyPerTurn++;
                        EventHandler.ValueChanged(ResourceType.Energy, energyPerTurn);
                    }
                    break;
                case TileType.MinusFun:
                    if (buildingPreset.buildingType == BuildingType.Fun)
                    {
                        funPerTurn--;
                        EventHandler.ValueChanged(ResourceType.Fun, funPerTurn);
                    }
                    break;
                case TileType.PlusFun:
                    if (buildingPreset.buildingType == BuildingType.Fun)
                    {
                        funPerTurn++;
                        EventHandler.ValueChanged(ResourceType.Fun, funPerTurn);
                    }
                    break;
            }
        }
    }
    private void AddBuildingProduction(BuildingPreset buildingPreset)
    {
        switch (buildingPreset.productionResource)
        {
            /*case ResourceType.Materials:
                materialsPerTurn += buildingPreset.productionResourcePerTurn;
                EventHandler.ValueChanged(ResourceType.Materials, currentMaterials, materialsPerTurn);
                break;
            */
            case ResourceType.Fun:
                funPerTurn += buildingPreset.productionResourcePerTurn;
                EventHandler.ValueChanged(ResourceType.Fun, funPerTurn);
                if (funPerTurn > funCap1 && !funCap1Reached || funPerTurn > funCap2)
                {
                    EventHandler.NewFunCapReached();
                    funCap1Reached = true;
                    break;
                }
                else break;
            case ResourceType.Oxygen:
                oxygenPerTurn += buildingPreset.productionResourcePerTurn;
                EventHandler.ValueChanged(ResourceType.Oxygen, oxygenPerTurn);
                break;
            case ResourceType.Energy:
                energyPerTurn += buildingPreset.productionResourcePerTurn;
                EventHandler.ValueChanged(ResourceType.Energy, energyPerTurn);
                break;
        }
    }
    private void AddBuildingMaintenance(BuildingPreset buildingPreset)
    {
        foreach (ResourceType resource in buildingPreset.maintenanceResource)
        {
            switch (resource)
            {
                case ResourceType.Oxygen:
                    oxygenPerTurn--;
                    EventHandler.ValueChanged(ResourceType.Oxygen, oxygenPerTurn);
                    break;
                case ResourceType.Energy:
                    energyPerTurn--;
                    EventHandler.ValueChanged(ResourceType.Energy, energyPerTurn);
                    break;
            }
        }
    }
    #endregion

    #region EndGame
    private void CheckEndGame()
    {
        if (energyPerTurn < 0)
        {
            EventHandler.EndGame(false, currentTurn, ResourceType.Energy);
        }
        else if (oxygenPerTurn < 0)
        {
            EventHandler.EndGame(false, currentTurn, ResourceType.Oxygen);
        }
        else if (funPerTurn >= maxFun)
        {
            EventHandler.EndGame(true, currentTurn, ResourceType.Fun);
        }
    }
    #endregion

    #region Events
    private void EventsSubscribe()
    {
        EventHandler.OnEndTurn += EndTurn;
        EventHandler.OnTryBuild += CheckBuildingConditions;
        EventHandler.OnBuildCompleted += ModifyValues_Building;
        EventHandler.OnBuildCompleted += ModifyValues_Tile;
        EventHandler.OnStartGame += GenerateValues;
    }
    private void OnDestroy()
    {
        EventHandler.OnEndTurn -= EndTurn;
        EventHandler.OnTryBuild -= CheckBuildingConditions;
        EventHandler.OnBuildCompleted -= ModifyValues_Building;
        EventHandler.OnBuildCompleted -= ModifyValues_Tile;
        EventHandler.OnStartGame -= GenerateValues;
    }
    #endregion
}

#region Enum
public enum BuildingType
{
    Base,
    Oxygen,
    Fun,
    Energy
}
public enum ResourceType
{
    Materials,
    Fun,
    Oxygen,
    Energy
}
public enum EndType
{
    Victory,
    DefeatDioxygen,
    DefeatEnergy
}
public enum TileType
{
    Neutral,
    MinusDioxygen,
    MinusEnergy,
    MinusFun,
    PlusDioxygen,
    PlusEnergy,
    PlusFun,
    NotConstructible
}
#endregion