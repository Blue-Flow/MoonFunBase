using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Building baseBuilding;
    public int currentTurn;
    public int maxFun = 20;
    public bool placingBuilding;
    public BuildingType curSelectedBuilding;

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

    void Awake ()
    {
        instance = this;
    }

    void Start ()
    {
        OnCreatedNewBuilding(baseBuilding);
        // updating the resource UI
        UI.instance.UpdateResourceText();
    }

    // called when the "End Turn" button is pressed
    public void EndTurn ()
    {
        // give resources
        currentFun += funPerTurn;
        currentMaterials += materialsPerTurn;
        currentOxygen += oxygenPerTurn;
        currentEnergy += energyPerTurn;

        // update the resource UI
        UI.instance.UpdateResourceText();

        CheckEndGame();

        currentTurn++;

        // enable usable tiles
        Map.instance.EnableUsableTiles();
    }

    private void CheckEndGame()
    {
        if (currentFun >= maxFun)
        {
            UI.instance.DisplayVictoryScreen(currentTurn);
        }
        else if (currentOxygen < 1)
        {
            UI.instance.DisplayGameOverScreen(ResourceType.Oxygen);
        }
        else if (currentEnergy < 1)
        {
            UI.instance.DisplayGameOverScreen(ResourceType.Energy);
        }
    }

    // called when we click on a building button to place it
    public void SetPlacingBuilding (BuildingType buildingType)
    {
        if (currentMaterials >= 1)
        {
            currentMaterials -= 1;
            placingBuilding = true;
            curSelectedBuilding = buildingType;
        }
        else
        {
            // Display a notification screen with the appropriate error : not enough materials to build
            UI.instance.DisplayNotification(0);
        }
    }

    // called when a new building has been created and placed down
    public void OnCreatedNewBuilding (Building building)
    {
        // resource the building produces
        if (building.doesProduceResource)
        {
            // Add the resources to give to the count
            AddBuildingProduction(building);
        }

        // resource the building may cost
        if (building.hasMaintenanceCost)
        {
            // Substract the resources to take from the count
            AddBuildingMaintenance(building);
        }

        placingBuilding = false;
        // update the resource UI
        UI.instance.UpdateResourceText();
    }

    private void AddBuildingMaintenance(Building building)
    {
        foreach (ResourceType resource in building.maintenanceResource)
        {
            int index = 0;
            switch (resource)
            {
                case ResourceType.Oxygen:
                    oxygenPerTurn -= building.maintenanceResourcePerTurn[index];
                    index++;
                    break;
                case ResourceType.Energy:
                    energyPerTurn -= building.maintenanceResourcePerTurn[index];
                    index++;
                    break;
            }
        }
    }

    private void AddBuildingProduction(Building building)
    {
        switch (building.productionResource)
        {
            case ResourceType.Materials:
                materialsPerTurn += building.productionResourcePerTurn;
                break;
            case ResourceType.Fun:
                funPerTurn += building.productionResourcePerTurn;
                break;
            case ResourceType.Oxygen:
                oxygenPerTurn += building.productionResourcePerTurn;
                break;
            case ResourceType.Energy:
                energyPerTurn += building.productionResourcePerTurn;
                break;
        }
    }
}