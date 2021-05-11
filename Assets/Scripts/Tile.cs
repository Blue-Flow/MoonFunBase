using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject highlight;
    public bool hasBuilding;
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
        return transform.position == potentialPosition && !hasBuilding;
    }
    public bool CanBeHighlighted(Vector2 potentialPosition)
    {
        return new Vector2(transform.position.x, transform.position.y) == potentialPosition && !hasBuilding;
    }

    void OnMouseDown ()
    {
        // place down a building on this tile
        if(GameManager.instance.placingBuilding && !hasBuilding && isEnabled)
            Map.instance.CreateNewBuilding(GameManager.instance.curSelectedBuilding, transform.position);
    }
}