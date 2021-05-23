using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    private BuildingPreset curSelectedBuilding;
    public bool placingBuilding;
    private Tile currentSelectedTile;

    private float indicatorUpdateRate = 0.05f;
    private float lastUpdateTime;
    //private Vector2 curIndicatorPosition;

    public GameObject placementIndicator;
    private void Start()
    {
        EventsSubscribe();
    }
    private void ConstructionStarted(BuildingPreset buildingPreset)
    {
        placingBuilding = true;
        //placementIndicator.SetActive(true);
        //placementIndicator.Transform.position = new Vector2(0, -99); // Place the indicator off-screen to avoid lag between apparition of the indicator and start of tracking its position
        curSelectedBuilding = buildingPreset;
    }
    private void Update()
    {
        if (placingBuilding && Input.GetMouseButtonDown(0))
        {
            GetSelectedTileInfo();
            //placementIndicator.transform.position = curIndicatorPosition;
            if (currentSelectedTile != null)
            {
                EventHandler.BuildCompleted(curSelectedBuilding, currentSelectedTile.tileType, currentSelectedTile.transform.position);
            }
        }

        /*if (Time.time - lastUpdateTime > indicatorUpdateRate)
        {
            lastUpdateTime = Time.time;
            // curIndicatorPosition = Selector.instance.GetCurTilePosition();

        }
        */

        if (placingBuilding && Input.GetKeyDown(KeyCode.Escape)
             || placingBuilding && Input.GetMouseButtonDown(1))
        {
            EventHandler.BuildOver();
        }
    }
    private void GetSelectedTileInfo()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            currentSelectedTile = hit.collider.GetComponent<Tile>();
        }
    }
    private void CancelBuildingConstruction()
    {
        placingBuilding = false;
        //placementIndicator.SetActive(false);
    }

    private void BuildCompleted(BuildingPreset buildingType, TileType tileType, Vector2 position)
    {
        currentSelectedTile.hasBuilding = true;
    }
    #region Events
    private void EventsSubscribe()
    {
        EventHandler.OnBuildOver += CancelBuildingConstruction;
        EventHandler.OnBuildStarted += ConstructionStarted;
        EventHandler.OnBuildCompleted += BuildCompleted;
    }
    private void EventsClear()
    {
        EventHandler.OnBuildOver -= CancelBuildingConstruction;
        EventHandler.OnBuildStarted -= ConstructionStarted;
        EventHandler.OnBuildCompleted -= BuildCompleted;
    }
    #endregion
}
