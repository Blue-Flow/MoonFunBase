using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctionnality : MonoBehaviour
{
    public void ClickEndTurnButton()
    {
        EventHandler.EndTurn();
    }
    public void ClickBuildingButton(BuildingPreset buildingPreset)
    {
        EventHandler.TryBuild(buildingPreset);
    }
    public void ClickCloseTips()
    {
        // Disable the tutorial when charging the game
        PlayerPrefs.SetInt("areTipsactive", 2);
    }
    public void OnClickActivateTips()
    {
        // Enable the tutorial from the menu
        EventHandler.SetTutorial();
    }
    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("Highscore", 0);
        UI.instance.highscoreText.text = (PlayerPrefs.GetInt("Highscore") + " turns");
    }
}
