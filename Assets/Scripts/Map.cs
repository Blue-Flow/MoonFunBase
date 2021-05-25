using System.Collections;
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
    [SerializeField] List<Tile> tilesPrefab = new List<Tile>();

    [SerializeField] List<Building> buildings = new List<Building>();

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

    public static Map instance;

    void Awake()
    {
        instance = this;
        EventsSubscribe();
    }
    void Start()
    {
        GenerateMap();
 
    }

    #region MapGeneration
    private void GenerateMap()
    {
        DetermineStartingTile();
        GenerateTilesinGrid();
    }

    private void GenerateTilesinGrid()
    {
        foreach (Tile tile in randomTilesList)
        {
            Vector2 tilePosition = tile.transform.position;
            tilesList.Remove(tile);
            Destroy(tile);
            int randomNumber = DetermineRandomTile();
            Tile randomTile = Instantiate(tilesPrefab[randomNumber], mapHolder.transform);
            randomTile.transform.position = tilePosition;
            tilesList.Add(randomTile);
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

    #endregion

    // displays the tiles which we can place a building on
    private void EnableUsableTiles (BuildingPreset buildingPreset)
    {
        foreach (Tile tile in tilesList)
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
    private void DisableUsableTiles ()
    {
        foreach(Tile tile in tilesList)
            tile.ToggleHighlight(false);
    }

    // creates a new building on a specific tile
    private void CreateNewBuilding (BuildingPreset buildingPreset, TileType tileType, Vector2 position)
    {
        GameObject buildingObj = Instantiate(buildingPreset.prefab, position, Quaternion.identity);
        buildings.Add(buildingObj.GetComponent<Building>());

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
        EventHandler.OnBuildStarted += EnableUsableTiles;
        EventHandler.OnBuildOver += DisableUsableTiles;
        EventHandler.OnBuildCompleted += CreateNewBuilding;
    }
    private void EventsClear()
    {
        EventHandler.OnBuildStarted -= EnableUsableTiles;
        EventHandler.OnBuildOver -= DisableUsableTiles;
        EventHandler.OnBuildCompleted -= CreateNewBuilding;
    }
    #endregion
}