using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventHandler : MonoBehaviour
{
    public static event Action OnEndTurn;
    public static event Action<bool, int, ResourceType> OnEndGame;
    public static event Action<int> OnError;

    public static event Action<BuildingType> OnTryBuild;
    public static event Action<BuildingType> OnBuildStarted;
    public static event Action OnBuildOver;
    public static event Action<BuildingType, TileType, Vector2>  OnBuildCompleted;
    public static event Action<ResourceType, int, int> OnValueChanged;

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
    public static void TryBuild(BuildingType buildingType)
    {
        if (OnTryBuild != null)
            OnTryBuild(buildingType);
        else Debug.Log("Error with event OnTryBuild, no subscriber");
    }
    public static void BuildStarted(BuildingType buildingType)
    {
        if (OnBuildStarted != null)
            OnBuildStarted(buildingType);
        else Debug.Log("Error with event OnBuildStarted, no subscriber");
    }
    public static void BuildOver()
    {
        if (OnBuildOver != null)
            OnBuildOver();
        else Debug.Log("Error with event OnBuildCanceled, no subscriber");
    }
    public static void BuildCompleted(BuildingType buildingType, TileType tileType, Vector2 tilePosition)
    {
        if (OnBuildCompleted != null)
            OnBuildCompleted(buildingType, tileType, tilePosition);
        else Debug.Log("Error with event OnBuildCompleted, no subscriber");
    }

    public static void ValueChanged(ResourceType resourceType, int currentResource, int resourcePerTurn)
    {
        if (OnValueChanged != null)
            OnValueChanged(resourceType, currentResource, resourcePerTurn);
        else Debug.Log("Error with event OnValueChanged, no subscriber");
    }
    public static void EndGame(bool victory, int turnNumber, ResourceType resource)
    {
        if (OnEndGame != null)
            OnEndGame(victory, turnNumber, resource);
        else Debug.Log("Error with event OnEndGame, no subscriber");
    }
}
