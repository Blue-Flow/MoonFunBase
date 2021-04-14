using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

        if(currentFun >= maxFun)
        {
            UI.instance.DisplayVictoryScreen(currentTurn);
        }

        currentTurn++;

        // enable the building buttons
        UI.instance.ToggleBuildingButtons(true);

        // enable usable tiles
        Map.instance.EnableUsableTiles();
    }

    // called when we click on a building button to place it
    public void SetPlacingBuilding (BuildingType buildingType)
    {
        placingBuilding = true;
        curSelectedBuilding = buildingType;
    }

    // called when a new building has been created and placed down
    public void OnCreatedNewBuilding (Building building)
    {
        // resource the building produces
        if(building.doesProduceResource)
        {
            switch(building.productionResource)
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

        // resource the building may cost
        if(building.hasMaintenanceCost)
        {
            switch(building.maintenanceResource)
            {
                case ResourceType.Fun:
                    funPerTurn -= building.maintenanceResourcePerTurn;
                    break;
                case ResourceType.Materials:
                    materialsPerTurn -= building.maintenanceResourcePerTurn;
                    break;
                case ResourceType.Oxygen:
                    oxygenPerTurn -= building.maintenanceResourcePerTurn;
                    break;
                case ResourceType.Energy:
                    energyPerTurn -= building.maintenanceResourcePerTurn;
                    break;
            }
        }

        placingBuilding = false;
        // update the resource UI
        UI.instance.UpdateResourceText();
    }
    
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}