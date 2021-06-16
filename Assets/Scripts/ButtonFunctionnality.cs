using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctionnality : MonoBehaviour
{
    public void ClickEndTurnButton()
    {
        EventHandler.EndTurn();
        EventHandler.ButtonClicked();
    }
    public void ClickBuildingButton(BuildingPreset buildingPreset)
    {
        EventHandler.TryBuild(buildingPreset);
        EventHandler.ButtonClicked();
    }
    public void ClickCloseTips()
    {
        // Disable the tutorial when charging the game
        PlayerPrefs.SetInt("areTipsactive", 2);
        EventHandler.ButtonClicked();
    }
    public void OnClickActivateTips()
    {
        // Enable the tutorial from the menu
        EventHandler.SetTutorial();
        EventHandler.ButtonClicked();
    }
    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("Highscore", 1000);
        UI.instance.highscoreText.text = (PlayerPrefs.GetInt("Highscore") + " turns");
        EventHandler.ButtonClicked();
    }
    public void PlayClickSound()
    {
        EventHandler.ButtonClicked();
    }
}
