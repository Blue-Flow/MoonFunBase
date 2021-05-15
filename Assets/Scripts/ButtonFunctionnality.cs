using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctionnality : MonoBehaviour
{
    public void OnEndTurnButton()
    {
        EventHandler.EndTurn();
    }
    public void OnClickSolarPanelButton()
    {
        EventHandler.TryBuild(BuildingType.Energy);
    }
    public void OnClickGreenhouseButton()
    {
        EventHandler.TryBuild(BuildingType.Oxygen);
    }
    public void OnClickFunhouseButton()
    {
        EventHandler.TryBuild(BuildingType.Fun);
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
