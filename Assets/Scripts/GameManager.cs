using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

public class GameManager : MonoBehaviour
{
    [SerializeField] Building baseBuilding;
    public int currentTurn;
    public int maxFun = 20;

    public bool placingBuilding;
    private BuildingType curSelectedBuilding;
    private int buildingConstructionCost;
    private Tile currentSelectedTile;

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
        EventsSubscribe();
    }

    void Start ()
    {
        // Finish the implementation of the base building (started in Map.cs)
        OnCreatedNewBuilding(baseBuilding);

        // Update the values on the UI
        UI.instance.UpdateTurnText(currentTurn);

        UpdateResourcesTexts();

        // Gets the registered setting
        if (PlayerPrefs.GetInt("areTipsactive") == 0)
            UI.instance.SetTipsActive(false);


    }
    private void Update()
    {
        if (placingBuilding && Input.GetMouseButtonDown(0) && (currentMaterials >= buildingConstructionCost))
        {
            GetSelectedTileInfo();
            if (currentSelectedTile != null)
            {
                EventHandler.BuildCompleted(curSelectedBuilding, currentSelectedTile.tileType, currentSelectedTile.transform.position);
                Debug.Log("Event triggered, warning, set currentSelectedTile to null again after that !");
            }
        }
        else if(placingBuilding && Input.GetKeyDown(KeyCode.Escape) 
             || placingBuilding && Input.GetMouseButtonDown(1))
        {
            EventHandler.BuildCanceled();
        }
    }

    private void GetSelectedTileInfo()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            currentSelectedTile = hit.collider.GetComponent<Tile>();
            Debug.Log(currentSelectedTile);
            Debug.Log(currentSelectedTile.tileType);
            Debug.Log(currentSelectedTile.transform.position);
        }

    }

    private void CancelBuildingConstruction()
    {
        placingBuilding = false;
        Map.instance.DisableUsableTiles();
        UI.instance.ToggleBuildingButtonHighlight(curSelectedBuilding, false);
    }

    // called when the "End Turn" button is pressed
    private void EndTurn ()
    {
        // give resources
        currentFun += funPerTurn;
        currentMaterials += materialsPerTurn;
        currentOxygen += oxygenPerTurn;
        currentEnergy += energyPerTurn;
        
        UpdateResourcesTexts();

        CheckEndGame();
        currentTurn++;
        UI.instance.UpdateTurnText(currentTurn);
    }

    private void UpdateResourcesTexts()
    {
        UI.instance.UpdateEnergyText(currentEnergy, energyPerTurn);
        UI.instance.UpdateOxygenText(currentOxygen, oxygenPerTurn);
        UI.instance.UpdateFunText(currentFun, funPerTurn);
        UI.instance.UpdateMaterialsText(currentMaterials, materialsPerTurn);
        UI.instance.UpdateFunBarAmount(((float)currentFun / (float)maxFun));
    }

    private void CheckEndGame()
    {
        if (currentEnergy < 1)
        {
            GetComponent<AudioSource>().Stop();
            UI.instance.DisplayGameOverScreen(ResourceType.Energy);
            EventsClear();
        }
        else if (currentOxygen < 1)
        {
            GetComponent<AudioSource>().Stop();
            UI.instance.DisplayGameOverScreen(ResourceType.Oxygen);
            EventsClear();
        }
        else if (currentFun >= maxFun)
        {
            // Stops the theme music from playing
            GetComponent<AudioSource>().Stop();
            UI.instance.DisplayVictoryScreen(currentTurn);
            EventsClear();
        }
    }

    // called when we click on a building button to place it
    private void SetPlacingBuilding (BuildingType buildingType)
    {
        EventHandler.OnBuildCanceled += CancelBuildingConstruction;
        if (currentMaterials >= 1)
        {
            placingBuilding = true;
            curSelectedBuilding = buildingType;
            Map.instance.EnableUsableTiles();
            UI.instance.ToggleBuildingButtonHighlight(buildingType, true);
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

        currentMaterials -= 1;
        placingBuilding = false;
        EventHandler.OnBuildCanceled -= CancelBuildingConstruction;
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
                    UI.instance.UpdateOxygenText(currentOxygen, oxygenPerTurn);
                    index++;
                    break;
                case ResourceType.Energy:
                    energyPerTurn -= building.maintenanceResourcePerTurn[index];
                    UI.instance.UpdateEnergyText(currentEnergy, energyPerTurn);
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
                UI.instance.UpdateMaterialsText(currentMaterials, materialsPerTurn);
                break;
            case ResourceType.Fun:
                funPerTurn += building.productionResourcePerTurn;
                UI.instance.UpdateFunText(currentFun, funPerTurn);
                break;
            case ResourceType.Oxygen:
                oxygenPerTurn += building.productionResourcePerTurn;
                UI.instance.UpdateOxygenText(currentOxygen, oxygenPerTurn);
                break;
            case ResourceType.Energy:
                energyPerTurn += building.productionResourcePerTurn;
                UI.instance.UpdateEnergyText(currentEnergy, energyPerTurn);
                break;
        }
    }
    #region Events
    private void EventsSubscribe()
    {
        EventHandler.OnEndTurn += EndTurn;
        EventHandler.OnBuildStarted += SetPlacingBuilding;
    }
    private void EventsClear()
    {
        EventHandler.OnEndTurn -= EndTurn;
        EventHandler.OnBuildStarted -= SetPlacingBuilding;
    }
    #endregion
}