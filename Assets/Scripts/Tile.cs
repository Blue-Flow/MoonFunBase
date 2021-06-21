using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject highlight;
    public GameObject randomTileType;
    public GameObject unknownTileIndicator;
    public GameObject warFog;
    public TileType tileType;
    public TilePreset tilePreset;
    public bool hasBuilding = false;
    public bool isEnabled = false;
    public bool isRandomTile = false;
    public bool isDiscovered = false;
    [SerializeField] Animation tileHighlightAnim;

    // toggles the tile highlight to show where we can place a building
    public void ToggleHighlight (bool toggle)
    {
        if (tileType != TileType.NotConstructible)
        {
            highlight.SetActive(toggle);
            isEnabled = toggle;
            if (isEnabled) tileHighlightAnim.Play();
        }
    }

    public void DiscoverTile()
    {
        warFog.SetActive(false);
        if (isRandomTile)
        {
            unknownTileIndicator.SetActive(false);
            randomTileType.SetActive(true);
        }
        isDiscovered = true;
        EventHandler.NewTileDiscovered(transform);
    }
    public void ShowRandomTileIndicator()
    {
        if (!isDiscovered)
        unknownTileIndicator.SetActive(true);
    }

    // can this tile be highlighted based on a given position
    public bool CanBeHighlighted (Vector3 potentialPosition)
    {
        return (transform.position == potentialPosition) && !hasBuilding;
    }
    public bool CanBeHighlighted(Vector2 potentialPosition)
    {
        return (new Vector2(transform.position.x, transform.position.y) == potentialPosition) && !hasBuilding;
    }
    /*public void RestartTile()
    {
        hasBuilding = false;
        isDiscovered = false;

        warFog.SetActive(true);
        unknownTileIndicator.SetActive(false);
        randomTileType.SetActive(false);
    }*/
}