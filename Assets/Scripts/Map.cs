﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private float tileSize = 1;
    [SerializeField] List<Tile> tilesList = new List<Tile>();
    [SerializeField] List<Tile> startTilesList = new List<Tile>();
    [SerializeField] List<Tile> randomTilesList = new List<Tile>();
    [SerializeField] GameObject mapHolder;

    [SerializeField] List<Building> buildingPrefabs = new List<Building>();
    [SerializeField] Tile tilesPrefab;

    [SerializeField] List<Building> buildings = new List<Building>();


    public static Map instance;

    void Awake()
    {
        instance = this;
        EventHandler.OnBuildCompleted += CreateNewBuilding;
    }

    void Start()
    {
        GenerateMap();
        EventsSubscribe();
    }

    private void GenerateMap()
    {
        GenerateTilesinGrid();
        DetermineStartingTile();
    }

    private void GenerateTilesinGrid()
    {
        //List<float> rotations = new List<float>() { 0, 90, 180, 270 };
                // set a random rotation for the tile -------------------------------- /!\ Pb rotation ? Liée à chaque prefab ?
                // float tilerotation = rotations[Random.Range(0, rotations.Count)];
                // tile.transform.rotation = new Quaternion(0, 0, tilerotation, 0);
    }

    private void DetermineStartingTile()
    {
        // determines the starting tile
        int randomNumber = Random.Range(0, startTilesList.Count);
        Tile startingTile = startTilesList[randomNumber];
        // sets the starting building
        startingTile.hasBuilding = true;
        Vector2 otherStartingPosition = new Vector2(startingTile.transform.position.x + 1, startingTile.transform.position.y);
        GetTileAtPosition(otherStartingPosition).hasBuilding = true;
        //startingTile.transform.rotation = new Quaternion(0, 0, 0, 0); // sets back the rotation of the first tile to keep building straight
        Building startBuilding = Instantiate(buildingPrefabs[0], startingTile.transform);
        startBuilding.transform.position = new Vector2(startingTile.transform.position.x + (tileSize / 2), startingTile.transform.position.y);
    }

    // displays the tiles which we can place a building on
    public void EnableUsableTiles ()
    {
        foreach(Tile tile in tilesList)
        {
            if (tile.hasBuilding && !tile.isEnabled)
            {
                Tile northTile = GetTileAtPosition(tile.transform.position + new Vector3(0, tileSize, 0));
                Tile eastTile = GetTileAtPosition(tile.transform.position + new Vector3(tileSize, 0, 0));
                Tile southTile = GetTileAtPosition(tile.transform.position + new Vector3(0, -tileSize, 0));
                Tile westTile = GetTileAtPosition(tile.transform.position + new Vector3(-tileSize, 0, 0));

                if (northTile != null)
                    northTile.ToggleHighlight(true);
                if (eastTile != null)
                    eastTile.ToggleHighlight(true);
                if (southTile != null)
                    southTile.ToggleHighlight(true);
                if (westTile != null)
                    westTile.ToggleHighlight(true);
            }
        }
    }

    // disables the tiles we can place a building on
    private void DisableUsableTiles ()
    {
        foreach(Tile tile in tilesList)
            tile.ToggleHighlight(false);
    }

    // creates a new building on a specific tile
    public void CreateNewBuilding (BuildingType buildingType, TileType tileType, Vector2 position)
    {
        Building prefabToSpawn = buildingPrefabs.Find(x => x.type == buildingType);
        GameObject buildingObj = Instantiate(prefabToSpawn.gameObject, position, Quaternion.identity);
        buildings.Add(buildingObj.GetComponent<Building>());
        
        Audio.instance.PlayConstructionSound(buildingType);
        UI.instance.DisableBuildingButtonHighlight(buildingType);

        GetTileAtPosition(position).hasBuilding = true;

        DisableUsableTiles();

        GameManager.instance.OnCreatedNewBuilding(prefabToSpawn);

    }

    // returns the tile that's at the given position
    private Tile GetTileAtPosition (Vector3 pos)
    {
        return tilesList.Find(x => x.CanBeHighlighted(pos));
    }
    private Tile GetTileAtPosition(Vector2 pos)
    {
        return tilesList.Find(x => x.CanBeHighlighted(pos));
    }
    #region Events
    private void EventsSubscribe()
    {
        //EventHandler.OnBuildStarted += ;
        EventHandler.OnBuildCanceled += DisableUsableTiles;
    }
    private void EventsClear()
    {
        //EventHandler.OnBuildStarted -= ;
        EventHandler.OnBuildCanceled -= DisableUsableTiles;
    }
    #endregion
}