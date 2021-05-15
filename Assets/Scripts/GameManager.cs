using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake ()
    {
        instance = this;
        EventsSubscribe();
    }

    private void Start ()
    {
        UI.instance.SetMaxFun(maxFun);
        // Finish the implementation of the base building (started in Map.cs)
        OnCreatedNewBuilding(baseBuilding);

        // Update the values on the UI
        UI.instance.UpdateTurnText(currentTurn);

        EventHandler.ValueChanged(ResourceType.Fun, currentFun, funPerTurn);
        EventHandler.ValueChanged(ResourceType.Oxygen, currentOxygen, oxygenPerTurn);
        EventHandler.ValueChanged(ResourceType.Energy, currentEnergy, energyPerTurn);
        EventHandler.ValueChanged(ResourceType.Materials, currentMaterials, materialsPerTurn);

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
                BuildCompleted();
            }
        }
        else if(placingBuilding && Input.GetKeyDown(KeyCode.Escape) 
             || placingBuilding && Input.GetMouseButtonDown(1))
        {
            EventHandler.BuildOver();
        }
    }

    private void BuildCompleted()
    {
        currentSelectedTile.hasBuilding = true;
    }

    private void GetSelectedTileInfo()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            currentSelectedTile = hit.collider.GetComponent<Tile>();
        }
    }

    private void CancelBuildingConstruction()
    {
        placingBuilding = false;
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
        // give resources
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
            // Clear events on all other components
        }
        else if (currentOxygen < 1)
        {
            EventHandler.EndGame(false, currentTurn, ResourceType.Oxygen);
            EventsClear();
            // Clear events on all other components
        }
        else if (currentFun >= maxFun)
        {
            EventHandler.EndGame(true, currentTurn, ResourceType.Fun);
            EventsClear();
            // Clear events on all other components
        }
    }

    // called when we click on a building button to place it
    private void SetPlacingBuilding (BuildingType buildingType)
    {
        if (currentMaterials >= 1)
        {
            placingBuilding = true;
            curSelectedBuilding = buildingType;
            EventHandler.BuildStarted(buildingType);
            EventHandler.OnBuildOver += CancelBuildingConstruction;
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
        EventHandler.BuildOver();
        EventHandler.OnBuildOver -= CancelBuildingConstruction;
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
                    EventHandler.ValueChanged(ResourceType.Oxygen, currentOxygen, oxygenPerTurn);
                    index++;
                    break;
                case ResourceType.Energy:
                    energyPerTurn -= building.maintenanceResourcePerTurn[index];
                    EventHandler.ValueChanged(ResourceType.Energy, currentEnergy, energyPerTurn);
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
                EventHandler.ValueChanged(ResourceType.Materials, currentMaterials, materialsPerTurn);
                break;
            case ResourceType.Fun:
                funPerTurn += building.productionResourcePerTurn;
                EventHandler.ValueChanged(ResourceType.Fun, currentFun, funPerTurn);
                break;
            case ResourceType.Oxygen:
                oxygenPerTurn += building.productionResourcePerTurn;
                EventHandler.ValueChanged(ResourceType.Oxygen, currentOxygen, oxygenPerTurn);
                break;
            case ResourceType.Energy:
                energyPerTurn += building.productionResourcePerTurn;
                EventHandler.ValueChanged(ResourceType.Energy, currentEnergy, energyPerTurn);
                break;
        }
    }
    #region Events
    private void EventsSubscribe()
    {
        EventHandler.OnEndTurn += EndTurn;
        EventHandler.OnTryBuild += SetPlacingBuilding;
    }
    private void EventsClear()
    {
        EventHandler.OnEndTurn -= EndTurn;
        EventHandler.OnTryBuild -= SetPlacingBuilding;
    }
    #endregion
}
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