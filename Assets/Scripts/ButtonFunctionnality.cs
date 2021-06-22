using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
    public void LoadTutoriel()
    {
        // Loads the tutoriel scene
        SceneManager.LoadScene(2);
        EventHandler.ButtonClicked();
    }
    public void LoadMainMenu()
    {
        // Loads the main menu scene
        SceneManager.LoadScene(0);
        EventHandler.ButtonClicked();
    }
    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("Highscore", 1000);
        UI.instance.highscoreText.text = (PlayerPrefs.GetInt("Highscore") + " turns");
        EventHandler.ButtonClicked();
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
        EventHandler.ButtonClicked();
    }
    public void PlayClickSound()
    {
        EventHandler.ButtonClicked();
    }
    public void QuitGame()
    {
        EventHandler.ButtonClicked();
        Application.Quit();
    }
}
