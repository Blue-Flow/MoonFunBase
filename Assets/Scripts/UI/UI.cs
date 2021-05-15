using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI : MonoBehaviour
{
    [Header("Graphismes")]
    [SerializeField] GameObject buildingButtons;

    [SerializeField] TextMeshProUGUI oxygenValue;
    [SerializeField] TextMeshProUGUI energyValue;
    [SerializeField] TextMeshProUGUI materialsValue;
    [SerializeField] TextMeshProUGUI funValue;
    private int maxFun;
    [SerializeField] Image funBar;
    [SerializeField] TextMeshProUGUI curTurnText;
    [SerializeField] TextMeshProUGUI baseName;

    [SerializeField] GameObject oxygenBuildingButtonHighlight;
    [SerializeField] GameObject energyBuildingButtonHighlight;
    [SerializeField] GameObject funBuildingButtonHighlight;

    [SerializeField] GameObject endScreen;
    [SerializeField] TextMeshProUGUI endText;
    [SerializeField] Sprite victoryBG;

    [SerializeField] Image notificationBG;
    [SerializeField] TextMeshProUGUI notificationText;
    [SerializeField] Canvas tipsCanvas;

    public static UI instance;

    void Awake ()
    {
        instance = this;
        EventsSubscribe();
    }

    void Start ()
    {
        GetBaseName();
    }
    public void SetMaxFun(int value) { maxFun = value; }
    private void GetBaseName()
    {
        baseName.text = PlayerPrefs.GetString("baseName", "MoonFunBase");
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

    public void DisplayNotification(int errorNumber)
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
    private void UpdateValueText(ResourceType resourceType, int currentResource, int resourcePerTurn)
    {
        string textToChange = string.Format("{0} ({1}{2})", currentResource, resourcePerTurn < 0 ? "" : "+", resourcePerTurn);
        switch (resourceType)
        {
            case ResourceType.Fun:
                funValue.text = textToChange;
                funBar.fillAmount = ((float)currentResource / (float)maxFun);
                break;
            case ResourceType.Materials:
                materialsValue.text = textToChange;
                break;
            case ResourceType.Oxygen:
                oxygenValue.text = textToChange;
                break;
            case ResourceType.Energy:
                energyValue.text = textToChange;
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
        }
        else
        {
            if (resource == ResourceType.Energy)
            {
                endText.text = ("You lost ! You ran out of energy..." + "\n" + "Your life support systems shut off and your people froze to death");
            }
            else
                endText.text = ("You lost ! You ran out of oxygen..." + "\n" + "Your people slowly died of suffocation");
        }
    }
    public void SetTipsActive(bool toggle)
    {
        tipsCanvas.gameObject.SetActive(toggle);
    }

    #region Events
    private void EventsSubscribe()
    {
        EventHandler.OnBuildStarted += EnableBuildingButtonHighlight;
        EventHandler.OnValueChanged += UpdateValueText;
        EventHandler.OnEndGame += DisplayEndScreen;
        //EventHandler.OnEndTurn += UpdateValueText;
        EventHandler.OnBuildOver += DisableBuildingButtonHighlight;
    }

    private void EnableBuildingButtonHighlight(BuildingType buildingType)
    {
        if(buildingType == BuildingType.Fun)
            funBuildingButtonHighlight.SetActive(true);
        else if (buildingType == BuildingType.Energy)
            energyBuildingButtonHighlight.SetActive(true);
        else if (buildingType == BuildingType.Oxygen)
            oxygenBuildingButtonHighlight.SetActive(true);
    }

    private void EventsClear()
    {
        EventHandler.OnValueChanged -= UpdateValueText;
        EventHandler.OnEndGame -= DisplayEndScreen;
        EventHandler.OnBuildOver -= DisableBuildingButtonHighlight;
    }
    #endregion
}