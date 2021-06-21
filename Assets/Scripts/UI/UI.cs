using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] TextMeshProUGUI oxygenValue;
    [SerializeField] TextMeshProUGUI energyValue;
    [SerializeField] TextMeshProUGUI materialsValue;
    [SerializeField] TextMeshProUGUI funValue;
    private int maxFun;
    [SerializeField] Image funBar;
    [SerializeField] TextMeshProUGUI curTurnText;

    [Header("Building")]
    [SerializeField] TextMeshProUGUI baseNameGameScreen;
    [SerializeField] GameObject oxygenBuildingButtonHighlight;
    [SerializeField] GameObject energyBuildingButtonHighlight;
    [SerializeField] GameObject funBuildingButtonHighlight;

    [Header("EndGame")]
    [SerializeField] GameObject endScreen;
    [SerializeField] TextMeshProUGUI endText;
    [SerializeField] Sprite victoryBG;
    [SerializeField] GameObject leaderboard;
    [SerializeField] TextMeshProUGUI baseNameEndScreen;
    public TextMeshProUGUI highscoreText;

    [SerializeField] Image notificationBG;
    [SerializeField] TextMeshProUGUI notificationText;

    public static UI instance;

    void Awake ()
    {
        instance = this;

        EventsSubscribe();
    }
    void Start ()
    {
        GetStartSettings();
    }
    public void SetMaxFun (int value) { maxFun = value; }
    private void GetStartSettings()
    {
        baseNameGameScreen.text = PlayerPrefs.GetString("baseName", "MoonFunBase");
        baseNameEndScreen.text = PlayerPrefs.GetString("baseName", "MoonFunBase");
        highscoreText.text = (PlayerPrefs.GetInt("Highscore") + " turns");
    }
    private void EnableBuildingButtonHighlight(BuildingPreset buildingPreset)
    {
        switch (buildingPreset.buildingType)
        {
            case BuildingType.Fun:
                funBuildingButtonHighlight.SetActive(true);
                oxygenBuildingButtonHighlight.SetActive(false);
                energyBuildingButtonHighlight.SetActive(false);
                break;
            case BuildingType.Energy:
                energyBuildingButtonHighlight.SetActive(true);
                oxygenBuildingButtonHighlight.SetActive(false);
                funBuildingButtonHighlight.SetActive(false);
                break;
            case BuildingType.Oxygen:
                oxygenBuildingButtonHighlight.SetActive(true);
                energyBuildingButtonHighlight.SetActive(false);
                funBuildingButtonHighlight.SetActive(false);
                break;
        }
    }
    private void DisableBuildingButtonHighlight(BuildingPreset buildingPreset)
    {
        funBuildingButtonHighlight.SetActive(false);
        energyBuildingButtonHighlight.SetActive(false);
        oxygenBuildingButtonHighlight.SetActive(false);
    }
    private void DisableBuildingButtonHighlight()
    {
        funBuildingButtonHighlight.SetActive(false);
        energyBuildingButtonHighlight.SetActive(false);
        oxygenBuildingButtonHighlight.SetActive(false);
    }
    public void UpdateTurnText(int currentTurn)
    {
        curTurnText.text = "Turn " + currentTurn;
    }

    private void DisplayNotification(int errorNumber)
    {
        notificationBG.gameObject.SetActive(true);
        switch (errorNumber)
        {
            // error 0 : not enough materials to build
            case 0:
                notificationText.text = ("You don't have enough materials to keep building !" + "\n" + "A new arrival is scheduled for next turn");
                break;
        }
    }
    private void UpdateValueText(ResourceType resourceType, int resourcePerTurn)
    {
        //string textToChange = string.Format("{0} ({1}{2})", currentResource, resourcePerTurn < 0 ? "" : "+", resourcePerTurn);
        switch (resourceType)
        {
            case ResourceType.Fun:
                funValue.text = resourcePerTurn + "(/" + maxFun +")";
                funBar.fillAmount = ((float)resourcePerTurn / (float)maxFun);
                break;
            case ResourceType.Materials:
                materialsValue.text = resourcePerTurn + "(+1)";
                break;
            case ResourceType.Oxygen:
                oxygenValue.text = Convert.ToString(resourcePerTurn);
                break;
            case ResourceType.Energy:
                energyValue.text = Convert.ToString(resourcePerTurn);
                break;
        }
    }
    private void DisplayEndScreen(bool victory, int turnNumber, ResourceType resource)
    {
        endScreen.SetActive(true);
        if (victory == true)
        {
            endScreen.GetComponent<Image>().sprite = victoryBG;
            endText.text = ("You won in " + turnNumber + " turns !");
            leaderboard.SetActive(true);
        }
        else
        {
            leaderboard.SetActive(false);
            if (resource == ResourceType.Energy)
            {
                endText.text = ("You lost ! You ran out of energy..." + "\n" + "Your life support systems shut off and your people froze to death");
            }
            else
                endText.text = ("You lost ! You ran out of oxygen..." + "\n" + "Your people slowly died of suffocation");
        }
    }
    private void CheckHighScore(bool victory, int currentTurn, ResourceType resource)
    {
        if (currentTurn < PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", currentTurn);
            highscoreText.text = (Convert.ToString(currentTurn) + " turns");
        }
    }

    #region Events
    private void EventsSubscribe()
    {
        EventHandler.OnBuildStarted += EnableBuildingButtonHighlight;
        EventHandler.OnValueChanged += UpdateValueText;
        EventHandler.OnEndGame += CheckHighScore;
        EventHandler.OnEndGame += DisplayEndScreen;
        EventHandler.OnBuildOver += DisableBuildingButtonHighlight;
        EventHandler.OnError += DisplayNotification;
    }
    #endregion
}