using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctionnality : MonoBehaviour
{
    public void OnEndTurnButton()
    {
        GameManager.instance.EndTurn();
    }
    public void OnClickSolarPanelButton()
    {
        GameManager.instance.SetPlacingBuilding(BuildingType.SolarPanel);
    }
    public void OnClickGreenhouseButton()
    {
        GameManager.instance.SetPlacingBuilding(BuildingType.Greenhouse);
    }
    public void OnClickFunhouseButton()
    {
        GameManager.instance.SetPlacingBuilding(BuildingType.Fun);
    }
    public void OnClickCloseTips()
    {
        PlayerPrefs.SetInt("areTipsactive", 0);
    }
    public void OnClickActivateTips()
    {
        UI.instance.SetTipsActive(true);
    }
}
