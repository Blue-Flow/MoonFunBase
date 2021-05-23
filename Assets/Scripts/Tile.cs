using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject highlight;
    public TileType tileType;
    public TilePreset tilePreset;
    public bool hasBuilding = false;
    public bool isEnabled = false;
    [SerializeField] Animation tileHighlightAnim;

    // toggles the tile highlight to show where we can place a building
    public void ToggleHighlight (bool toggle)
    {
        highlight.SetActive(toggle);
        isEnabled = toggle;
        if(isEnabled) tileHighlightAnim.Play();
    }

    // can this tile be highlighted based on a given position
    public bool CanBeHighlighted (Vector3 potentialPosition)
    {
        return (transform.position == potentialPosition) && !hasBuilding && tileType != TileType.NotConstructible;
    }
    public bool CanBeHighlighted(Vector2 potentialPosition)
    {
        return (new Vector2(transform.position.x, transform.position.y) == potentialPosition) && !hasBuilding && tileType != TileType.NotConstructible;
    }
}