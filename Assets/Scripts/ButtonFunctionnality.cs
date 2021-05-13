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
        EventHandler.BuildCanceled();
        EventHandler.BuildStarted(BuildingType.SolarPanel);
    }
    public void OnClickGreenhouseButton()
    {
        EventHandler.BuildCanceled();
        EventHandler.BuildStarted(BuildingType.Greenhouse);
    }
    public void OnClickFunhouseButton()
    {
        EventHandler.BuildCanceled();
        EventHandler.BuildStarted(BuildingType.Fun);
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
