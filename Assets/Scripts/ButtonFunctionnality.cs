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
    public void ClickCloseTips()
    {
        // Disable the tutorial when charging the game
        PlayerPrefs.SetInt("areTipsactive", 2);
        EventHandler.ButtonClicked();
    }
    public void LoadTutoriel()
    {
        EventHandler.ClearGame();
        // Loads the tutoriel scene
        SceneManager.LoadScene(2, LoadSceneMode.Single);
        EventHandler.ButtonClicked();
    }
    public void LoadMainMenu()
    {
        // Loads the main menu scene
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        EventHandler.ClearGame();
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
        //EventHandler.ClearGame();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        //EventHandler.StartGame();
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
