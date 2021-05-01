using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    [Header("Audio")]
    [SerializeField] AudioMixer audioMixer;

    public static GameManager instance;

    void Awake ()
    {
        instance = this;
    }

    void Start ()
    {
        // Finish the implementation of the base building (started in Map.cs)
        OnCreatedNewBuilding(baseBuilding);

        // Update the values on the UI
        UI.instance.UpdateFunBarAmount(((float)GameManager.instance.currentFun / (float)GameManager.instance.maxFun));
        UI.instance.UpdateTurnText(currentTurn);
        UI.instance.UpdateEnergyText(currentEnergy, energyPerTurn);
        UI.instance.UpdateOxygenText(currentOxygen, oxygenPerTurn);
        UI.instance.UpdateFunText(currentFun, funPerTurn);
        UI.instance.UpdateMaterialsText(currentMaterials, materialsPerTurn);

        // Gets the registered settings
        float volume = PlayerPrefs.GetFloat("volume");
        audioMixer.SetFloat("volume", volume);


    }

    // called when the "End Turn" button is pressed
    public void EndTurn ()
    {
        // give resources
        currentFun += funPerTurn;
        currentMaterials += materialsPerTurn;
        currentOxygen += oxygenPerTurn;
        currentEnergy += energyPerTurn;

        UI.instance.UpdateEnergyText(currentEnergy, energyPerTurn);
        UI.instance.UpdateOxygenText(currentOxygen, oxygenPerTurn);
        UI.instance.UpdateFunText(currentFun, funPerTurn);
        UI.instance.UpdateMaterialsText(currentMaterials, materialsPerTurn);

        UI.instance.UpdateFunBarAmount(((float)GameManager.instance.currentFun / (float)GameManager.instance.maxFun));
        CheckEndGame();
        currentTurn++;
        UI.instance.UpdateTurnText(currentTurn);
    }

    private void CheckEndGame()
    {
        if (currentFun >= maxFun)
        {
            // Stops the theme music from playing
            GetComponent<AudioSource>().Stop();
            UI.instance.DisplayVictoryScreen(currentTurn);
        }
        else if (currentOxygen < 1)
        {
            GetComponent<AudioSource>().Stop();
            UI.instance.DisplayGameOverScreen(ResourceType.Oxygen);
        }
        else if (currentEnergy < 1)
        {
            GetComponent<AudioSource>().Stop();
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

        placingBuilding = false;
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
}