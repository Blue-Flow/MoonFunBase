using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctionnality : MonoBehaviour
{
    public void OnEndTurnButton()
    {
        EventHandler.EndTurn();
    }
    public void ClickBuildingButton(BuildingPreset buildingPreset)
    {
        EventHandler.TryBuild(buildingPreset);
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
