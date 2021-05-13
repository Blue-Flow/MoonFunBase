using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventHandler : MonoBehaviour
{
    public static event Action OnEndTurn;

    public static event Action<BuildingType> OnBuildStarted;
    public static event Action OnBuildCanceled;
    public static event Action<BuildingType, TileType, Vector2>  OnBuildCompleted;
    public static void EndTurn()
    {
        if (OnEndTurn != null)
            OnEndTurn();
    }
    public static void BuildStarted(BuildingType building)
    {
        if (OnBuildStarted != null)
            OnBuildStarted(building);
    }
    public static void BuildCanceled()
    {
        if (OnBuildCanceled != null)
            OnBuildCanceled();
    }
    public static void BuildCompleted(BuildingType buildingType, TileType tileType, Vector2 tilePosition)
    {
        if (OnBuildCompleted != null)
            OnBuildCompleted(buildingType, tileType, tilePosition);
    }
}
