using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventHandler : MonoBehaviour
{
    public static event Action OnStartGame;
    public static event Action OnSetTutorial;

    public static event Action OnEndTurn;
    public static event Action<bool, int, ResourceType> OnEndGame;
    public static event Action OnClearGame;
    public static event Action<int> OnError;

    public static event Action<BuildingPreset> OnTryBuild;
    public static event Action<BuildingPreset> OnBuildStarted;
    public static event Action OnBuildOver;
    public static event Action<BuildingPreset, TileType, Vector2>  OnBuildCompleted;
    public static event Action<ResourceType, int> OnValueChanged;

    private void Awake()
    {
        int eventHandlerCount = FindObjectsOfType<EventHandler>().Length;
        if (eventHandlerCount > 1) { Destroy(gameObject); }
        else DontDestroyOnLoad(gameObject);
    }
    public static void StartGame()
    {
        if (OnStartGame != null)
        {
            OnStartGame();
        }
        else Debug.Log("Error with event OnStartGame, no subscriber");
    }
    // Sets the currentStep to the first
    // Gets the currentStep info
    // Displays the currentStep
    // Puts the tutorial screen to on
    public static void SetTutorial()
    {
        if (OnSetTutorial != null)
            OnSetTutorial();
        else Debug.Log("Error with event OnSetTutorial, no subscriber");
    }
    public static void Error(int errorNumber)
    {
        if (OnError != null)
            OnError(errorNumber);
        else Debug.Log("Error with event OnError, no subscriber");
    }
    public static void EndTurn()
    {
        if (OnEndTurn != null)
            OnEndTurn();
        else Debug.Log("Error with event OnEndTurn, no subscriber");
    }
    // Called by the UI buttons
    // Check if there is enough materials to build
    public static void TryBuild(BuildingPreset buildingType)
    {
        if (OnTryBuild != null)
            OnTryBuild(buildingType);
        else Debug.Log("Error with event OnTryBuild, no subscriber");
    }
    // Called if conditions to build are checked true
    public static void BuildStarted(BuildingPreset buildingPreset)
    {
        if (OnBuildStarted != null)
            OnBuildStarted(buildingPreset);
        else Debug.Log("Error with event OnBuildStarted, no subscriber");
    }
    public static void BuildOver()
    {
        if (OnBuildOver != null)
            OnBuildOver();
        else Debug.Log("Error with event OnBuildCanceled, no subscriber");
    }
    // Called if mouse clic on a valid tile
    // Changes the tile status to hasBuilding
    // Spawns the prefab on the tile
    // Calls for the update of the resources values
    // Updates the UI accordingly to the new numbers for the resources
    // Disable the building indicators (tilesHighlight, buildingIndicator, buttonHighlight)
    public static void BuildCompleted(BuildingPreset buildingPreset, TileType tileType, Vector2 tilePosition)
    {
        if (OnBuildCompleted != null)
            OnBuildCompleted(buildingPreset, tileType, tilePosition);
        else Debug.Log("Error with event OnBuildCompleted, no subscriber");
    }

    public static void ValueChanged(ResourceType resourceType, int resourcePerTurn)
    {
        if (OnValueChanged != null)
            OnValueChanged(resourceType, resourcePerTurn);
        else Debug.Log("Error with event OnValueChanged, no subscriber");
    }
    public static void EndGame(bool victory, int turnNumber, ResourceType resource)
    {
        if (OnEndGame != null)
            OnEndGame(victory, turnNumber, resource);
        else Debug.Log("Error with event OnEndGame, no subscriber");
    }
    public static void ClearGame()
    {
        if (OnClearGame != null)
            OnClearGame();
        else Debug.Log("Error with event OnClearGame, no subscriber");
    }
}
