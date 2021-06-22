using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private float tileSize = 1;
    [SerializeField] List<Tile> generation_TilesList = new List<Tile>();
    [SerializeField] List<Tile> startTilesList = new List<Tile>();
    [SerializeField] List<Tile> generation_RandomTilesList = new List<Tile>();
    [SerializeField] GameObject mapHolder;

    [SerializeField] List<Building> buildingPrefabs = new List<Building>();
    [SerializeField] List<Tile> tilesPrefab = new List<Tile>();

    [SerializeField] List<Tile> thisGameTilesList = new List<Tile>();
    [SerializeField] List<Tile> thisGameRandomTilesList = new List<Tile>();

    [SerializeField] List<Building> buildingsList = new List<Building>();
    
    private Building startBuilding = null;

    // Counts for random Tile generation
    private int tileMECount;
    private int tileMOCount;
    private int tileMFCount;
    private int tilePECount;
    private int tilePOCount;
    private int tilePFCount;
    private int tileNCCount;
    private int tileMaxCount = 9;
    private bool randomTilePossible = true;

    void Awake()
    {
        EventsSubscribe();
    }

    #region MapGeneration
    private void GenerateMap()
    {
        GenerateGlobalTilesinGrid();
        GenerateRandomTilesinGrid();
        DetermineStartingTile();
        ShowTilesType();
    }
    private void GenerateGlobalTilesinGrid()
    {
        foreach (Tile tile in generation_TilesList)
        {
            thisGameTilesList.Add(tile);
        }
    }
    #region RandomTilesGeneration
    private void GenerateRandomTilesinGrid()
    {
        foreach (Tile tile in generation_RandomTilesList)
        {
            if (tile == null)
            {
                Debug.Log("Prout");
                continue;
            }
            Vector2 tilePosition = tile.transform.position;
            tile.gameObject.SetActive(false);
            thisGameTilesList.Remove(tile);
            int randomNumber = DetermineRandomTile();
            Tile randomTile = Instantiate(tilesPrefab[randomNumber], mapHolder.transform);
            randomTile.transform.position = tilePosition;
            randomTile.isRandomTile = true;
            thisGameTilesList.Add(randomTile);
            thisGameRandomTilesList.Add(randomTile);
        }
    }
    private int DetermineRandomTile()
    {
        int tryNumber = 0;
        Start:
        int randomNumber = Random.Range(0, tilesPrefab.Count);
        bool possible = CheckRandomTilePossibility(randomNumber);
        if (possible)
            return randomNumber;
        else
        { 
            if (tryNumber < 30)
            {
                tryNumber++;
                goto Start;
            }
            else
            {
                Debug.Log("Generation failed");
                tileMOCount++;
                return 0;
            }
        }
    }
    private bool CheckRandomTilePossibility(int tileNumber)
    {
        Tile prefabToSpawn = tilesPrefab[tileNumber];
        switch (prefabToSpawn.tileType)
        {
            case TileType.MinusDioxygen:
                randomTilePossible = (tileMOCount < tileMaxCount) ? true : false;
                if (randomTilePossible) tileMOCount++;
                break;

            case TileType.MinusEnergy:
                randomTilePossible = (tileMECount < tileMaxCount) ? true : false;
                if (randomTilePossible) tileMECount++;
                break;

            case TileType.MinusFun:
                randomTilePossible = (tileMFCount < tileMaxCount) ? true : false;
                if (randomTilePossible) tileMFCount++;
                break;

            case TileType.NotConstructible:
                randomTilePossible = (tileNCCount < tileMaxCount) ? true : false;
                if (randomTilePossible) tileNCCount++;
                break;

            case TileType.PlusDioxygen:
                randomTilePossible = (tilePOCount < tileMaxCount) ? true : false;
                if (randomTilePossible) tilePOCount++;
                break;

            case TileType.PlusEnergy:
                randomTilePossible = (tilePECount < tileMaxCount) ? true : false;
                if (randomTilePossible) tilePECount++;
                break;

            case TileType.PlusFun:
                randomTilePossible = (tilePFCount < tileMaxCount) ? true : false;
                if (randomTilePossible) tilePFCount++;
                break;
            default:
                Debug.Log("Invalid tile type");
                randomTilePossible = false;
                break;
        }
        return randomTilePossible;
    }
    #endregion
    private void DetermineStartingTile()
    {
        // determines the starting tile
        int randomNumber = Random.Range(0, startTilesList.Count);
        Tile startingTile = startTilesList[randomNumber];

        // sets the starting building
        startingTile.hasBuilding = true;
        startingTile.warFog.SetActive(false);
        Vector2 otherStartingPosition = new Vector2(startingTile.transform.position.x + 1, startingTile.transform.position.y);
        Tile otherstartingTile = GetTileAtPosition(otherStartingPosition);
        otherstartingTile.hasBuilding = true;
        otherstartingTile.warFog.SetActive(false);
        //startingTile.transform.rotation = new Quaternion(0, 0, 0, 0); // sets back the rotation of the first tile to keep building straight
        startBuilding = Instantiate(buildingPrefabs[0], startingTile.transform);
        startBuilding.transform.position = new Vector2(startingTile.transform.position.x + (tileSize / 2), startingTile.transform.position.y);
    }

    #endregion

    // displays the tiles which we can place a building on
    private void EnableUsableTiles (BuildingPreset buildingPreset)
    {
        foreach (Tile tile in thisGameTilesList)
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
    private void ShowRandomTilesType(BuildingPreset buildingPreset, TileType tileType, Vector2 tilePosition)
    {
        foreach (Tile tile in thisGameTilesList)
        {
            if (tile.hasBuilding)
            {
                Tile northTile = GetTileAtPosition(tile.transform.position + new Vector3(0, tileSize, 0));
                Tile eastTile = GetTileAtPosition(tile.transform.position + new Vector3(tileSize, 0, 0));
                Tile southTile = GetTileAtPosition(tile.transform.position + new Vector3(0, -tileSize, 0));
                Tile westTile = GetTileAtPosition(tile.transform.position + new Vector3(-tileSize, 0, 0));

                if (northTile != null && !northTile.isDiscovered)
                    northTile.DiscoverTile();
                if (eastTile != null && !eastTile.isDiscovered)
                    eastTile.DiscoverTile();
                if (southTile != null && !southTile.isDiscovered)
                    southTile.DiscoverTile();
                if (westTile != null && !westTile.isDiscovered)
                    westTile.DiscoverTile();
            }
        }
    }
    private void ShowTilesType()
    {
        foreach (Tile tile in thisGameTilesList)
        {
            if (tile.hasBuilding)
            {
                Tile northTile = GetTileAtPosition(tile.transform.position + new Vector3(0, tileSize, 0));
                Tile eastTile = GetTileAtPosition(tile.transform.position + new Vector3(tileSize, 0, 0));
                Tile southTile = GetTileAtPosition(tile.transform.position + new Vector3(0, -tileSize, 0));
                Tile westTile = GetTileAtPosition(tile.transform.position + new Vector3(-tileSize, 0, 0));

                if (northTile != null && !northTile.isDiscovered)
                    northTile.DiscoverTile();
                if (eastTile != null && !eastTile.isDiscovered)
                    eastTile.DiscoverTile();
                if (southTile != null && !southTile.isDiscovered)
                    southTile.DiscoverTile();
                if (westTile != null && !westTile.isDiscovered)
                    westTile.DiscoverTile();
            }
        }
    }
    private void ShowRandomTilesIndicator(Transform tilePosition)
    {
        Tile northTile = GetTileAtPosition(tilePosition.transform.position + new Vector3(0, tileSize, 0));
        Tile eastTile = GetTileAtPosition(tilePosition.transform.position + new Vector3(tileSize, 0, 0));
        Tile southTile = GetTileAtPosition(tilePosition.transform.position + new Vector3(0, -tileSize, 0));
        Tile westTile = GetTileAtPosition(tilePosition.transform.position + new Vector3(-tileSize, 0, 0));

        if (northTile != null && northTile.isRandomTile)
            northTile.ShowRandomTileIndicator();
        if (eastTile != null && eastTile.isRandomTile)
            eastTile.ShowRandomTileIndicator();
        if (southTile != null && southTile.isRandomTile)
            southTile.ShowRandomTileIndicator();
        if (westTile != null && westTile.isRandomTile)
            westTile.ShowRandomTileIndicator();
    }
    private void DisableUsableTiles ()
    {
        ShowTilesType();
        foreach(Tile tile in thisGameTilesList)
            tile.ToggleHighlight(false);
    }
    // creates a new building on a specific tile
    private void CreateNewBuilding (BuildingPreset buildingPreset, TileType tileType, Vector2 position)
    {
        GameObject buildingObj = Instantiate(buildingPreset.prefab, position, Quaternion.identity);
        buildingsList.Add(buildingObj.GetComponent<Building>());
    }
    // returns the tile that's at the given position
    private Tile GetTileAtPosition (Vector3 pos)
    {
        return thisGameTilesList.Find(x => x.CanBeHighlighted(pos));
    }
    private Tile GetTileAtPosition(Vector2 pos)
    {
        return thisGameTilesList.Find(x => x.CanBeHighlighted(pos));
    }
    #region Events
    private void EventsSubscribe()
    {
        EventHandler.OnStartGame += GenerateMap;
        EventHandler.OnBuildStarted += EnableUsableTiles;
        EventHandler.OnBuildOver += DisableUsableTiles;
        EventHandler.OnBuildCompleted += CreateNewBuilding;
        EventHandler.OnBuildCompleted += ShowRandomTilesType;
        EventHandler.OnNewTileDiscovered += ShowRandomTilesIndicator;
    }
    private void OnDestroy()
    {
        EventHandler.OnStartGame -= GenerateMap;
        EventHandler.OnBuildStarted -= EnableUsableTiles;
        EventHandler.OnBuildOver -= DisableUsableTiles;
        EventHandler.OnBuildCompleted -= CreateNewBuilding;
        EventHandler.OnBuildCompleted -= ShowRandomTilesType;
        EventHandler.OnNewTileDiscovered -= ShowRandomTilesIndicator;
    }
    #endregion
}