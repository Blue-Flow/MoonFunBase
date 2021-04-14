using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] List<Tile> tiles = new List<Tile>();
    [SerializeField] List<Building> buildings = new List<Building>();

    private float tileSize = 1;

    [SerializeField] List<Building> buildingPrefabs = new List<Building>();

    public static Map instance;

    void Awake ()
    {
        instance = this;
    }

    void Start ()
    {
        EnableUsableTiles();
    }

    // displays the tiles which we can place a building on
    public void EnableUsableTiles ()
    {
        foreach(Tile tile in tiles)
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
    public void DisableUsableTiles ()
    {
        foreach(Tile tile in tiles)
            tile.ToggleHighlight(false);
    }

    // creates a new building on a specific tile
    public void CreateNewBuilding (BuildingType buildingType, Vector3 position)
    {
        Building prefabToSpawn = buildingPrefabs.Find(x => x.type == buildingType);
        GameObject buildingObj = Instantiate(prefabToSpawn.gameObject, position, Quaternion.identity);
        buildings.Add(buildingObj.GetComponent<Building>());

        GetTileAtPosition(position).hasBuilding = true;

        DisableUsableTiles();

        GameManager.instance.OnCreatedNewBuilding(prefabToSpawn);

    }

    // returns the tile that's at the given position
    Tile GetTileAtPosition (Vector3 pos)
    {
        return tiles.Find(x => x.CanBeHighlighted(pos));
    }
}