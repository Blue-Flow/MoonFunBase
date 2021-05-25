using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Building baseBuilding;
    public int currentTurn;
    public int maxFun = 20;

    //public bool placingBuilding;
    //private Tile currentSelectedTile;
    //private int buildingConstructionCost;

    [Header("Current Resources")]
    public int currentFun;
    public int currentMaterials;
    public int currentOxygen;
    public int currentEnergy;

    [Header("Round Resource Increase")]
    public int funPerTurn;
    public int materialsPerTurn;
    public int oxygenPerTurn;
    public int energyPerTurn;

    public static GameManager instance;

    private void Awake ()
    {
        instance = this;
        EventsSubscribe();
    }

    private void Start()
    {
        UI.instance.SetMaxFun(maxFun);
        // Finish the implementation of the base building (started in Map.cs) TODO : le remettre ? Comment ?

        // Update the values on the UI
        UI.instance.UpdateTurnText(currentTurn);

        EventHandler.ValueChanged(ResourceType.Fun, currentFun, funPerTurn);
        EventHandler.ValueChanged(ResourceType.Oxygen, currentOxygen, oxygenPerTurn);
        EventHandler.ValueChanged(ResourceType.Energy, currentEnergy, energyPerTurn);
        EventHandler.ValueChanged(ResourceType.Materials, currentMaterials, materialsPerTurn);
    }

    // called when the "End Turn" button is pressed
    private void EndTurn ()
    {
        GiveResources();

        CheckEndGame();

        currentTurn++;
        UI.instance.UpdateTurnText(currentTurn);
    }
    private void GiveResources()
    {
        if (funPerTurn != 0)
        {
            currentFun += funPerTurn;
            EventHandler.ValueChanged(ResourceType.Fun, currentFun, funPerTurn);
        }
        if (energyPerTurn != 0)
        {
            currentEnergy += energyPerTurn;
            EventHandler.ValueChanged(ResourceType.Energy, currentEnergy, energyPerTurn);
        }
        if (oxygenPerTurn != 0)
        {
            currentOxygen += oxygenPerTurn;
            EventHandler.ValueChanged(ResourceType.Oxygen, currentOxygen, oxygenPerTurn);
        }
        currentMaterials += materialsPerTurn;
        EventHandler.ValueChanged(ResourceType.Materials, currentMaterials, materialsPerTurn);
    }

    private void CheckEndGame()
    {
        if (currentEnergy < 1)
        {
            EventHandler.EndGame(false, currentTurn, ResourceType.Energy);
            EventsClear();
            // TODO Clear events on all other components
        }
        else if (currentOxygen < 1)
        {
            EventHandler.EndGame(false, currentTurn, ResourceType.Oxygen);
            EventsClear();
            // TODO Clear events on all other components
        }
        else if (currentFun >= maxFun)
        {
            CheckHighScore();
            EventHandler.EndGame(true, currentTurn, ResourceType.Fun);
            EventsClear();
            // TODO Clear events on all other components
        }
    }
    private void CheckHighScore()
    {
        if (currentTurn < PlayerPrefs.GetInt("Highscore"))
            PlayerPrefs.SetInt("Highscore", currentTurn);
    }

    // called to check possibility of the building placement
    private void CheckBuildingConditions (BuildingPreset buildingPreset)
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
        EventHandler.ValueChanged(ResourceType.Materials, currentMaterials, materialsPerTurn);
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
                        EventHandler.ValueChanged(ResourceType.Oxygen, currentOxygen, oxygenPerTurn);
                    }
                    break;
                case TileType.PlusDioxygen:
                    if (buildingPreset.buildingType == BuildingType.Oxygen)
                    {
                        oxygenPerTurn++;
                        EventHandler.ValueChanged(ResourceType.Oxygen, currentOxygen, oxygenPerTurn);
                    }
                    break;
                case TileType.MinusEnergy:
                    if (buildingPreset.buildingType == BuildingType.Energy)
                    {
                        energyPerTurn--;
                        EventHandler.ValueChanged(ResourceType.Energy, currentEnergy, energyPerTurn);
                    }
                    break;
                case TileType.PlusEnergy:
                    if (buildingPreset.buildingType == BuildingType.Energy)
                    {
                        energyPerTurn++;
                        EventHandler.ValueChanged(ResourceType.Energy, currentEnergy, energyPerTurn);
                    }
                    break;
                case TileType.MinusFun:
                    if (buildingPreset.buildingType == BuildingType.Fun)
                    {
                        funPerTurn--;
                        EventHandler.ValueChanged(ResourceType.Fun, currentFun, funPerTurn);
                    }
                    break;
                case TileType.PlusFun:
                    if (buildingPreset.buildingType == BuildingType.Fun)
                    {
                        funPerTurn++;
                        EventHandler.ValueChanged(ResourceType.Fun, currentFun, funPerTurn);
                    }
                    break;
            }
        }
    }
    private void AddBuildingProduction(BuildingPreset buildingPreset)
    {
        switch (buildingPreset.productionResource)
        {
            case ResourceType.Materials:
                materialsPerTurn += buildingPreset.productionResourcePerTurn;
                EventHandler.ValueChanged(ResourceType.Materials, currentMaterials, materialsPerTurn);
                break;
            case ResourceType.Fun:
                funPerTurn += buildingPreset.productionResourcePerTurn;
                EventHandler.ValueChanged(ResourceType.Fun, currentFun, funPerTurn);
                break;
            case ResourceType.Oxygen:
                oxygenPerTurn += buildingPreset.productionResourcePerTurn;
                EventHandler.ValueChanged(ResourceType.Oxygen, currentOxygen, oxygenPerTurn);
                break;
            case ResourceType.Energy:
                energyPerTurn += buildingPreset.productionResourcePerTurn;
                EventHandler.ValueChanged(ResourceType.Energy, currentEnergy, energyPerTurn);
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
                    EventHandler.ValueChanged(ResourceType.Oxygen, currentOxygen, oxygenPerTurn);
                    break;
                case ResourceType.Energy:
                    energyPerTurn--;
                    EventHandler.ValueChanged(ResourceType.Energy, currentEnergy, energyPerTurn);
                    break;
            }
        }
    }

    #region Events
    private void EventsSubscribe()
    {
        EventHandler.OnEndTurn += EndTurn;
        EventHandler.OnTryBuild += CheckBuildingConditions;
        EventHandler.OnBuildCompleted += ModifyValues_Building;
        EventHandler.OnBuildCompleted += ModifyValues_Tile;
    }

    private void EventsClear()
    {
        EventHandler.OnEndTurn -= EndTurn;
        EventHandler.OnTryBuild -= CheckBuildingConditions;
        EventHandler.OnBuildCompleted -= ModifyValues_Building;
        EventHandler.OnBuildCompleted -= ModifyValues_Tile;
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