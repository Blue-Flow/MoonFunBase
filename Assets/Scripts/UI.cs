using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject buildingButtons;

    [SerializeField] TextMeshProUGUI oxygenValue;
    [SerializeField] TextMeshProUGUI energyValue;
    [SerializeField] TextMeshProUGUI materialsValue;
    [SerializeField] Image funBar;

    [SerializeField] Image endScreenBG;
    [SerializeField] TextMeshProUGUI endText;
    [SerializeField] Sprite victorySprite;

    [SerializeField] Image notificationBG;
    [SerializeField] TextMeshProUGUI notificationText;

    [SerializeField] TextMeshProUGUI curTurnText;

    public static UI instance;

    void Awake ()
    {
        instance = this;
    }

    void Start ()
    {
        curTurnText.text = "Turn " + GameManager.instance.currentTurn;
    }

    // called when the "End Turn" button is pressed
    public void OnEndTurnButton ()
    {
        GameManager.instance.EndTurn();
        curTurnText.text = "Turn " + GameManager.instance.currentTurn;
    }

    // called when we click the solar panel button
    public void OnClickSolarPanelButton ()
    {
        GameManager.instance.SetPlacingBuilding(BuildingType.SolarPanel);

    }

    // called when we click the greenhouse button
    public void OnClickGreenhouseButton()
    {
        GameManager.instance.SetPlacingBuilding(BuildingType.Greenhouse);
    }

    // called when we click the mine button
    public void OnClickFunhouseButton()
    { 
         GameManager.instance.SetPlacingBuilding(BuildingType.Fun);
    }

    // called when we place a building or the turn has ended
    public void UpdateResourceText ()
    {
        string materials = string.Format("{0} ({1}{2})", GameManager.instance.currentMaterials, GameManager.instance.materialsPerTurn < 0 ? "" : "+", GameManager.instance.materialsPerTurn);
        string oxygen = string.Format("{0} ({1}{2})", GameManager.instance.currentOxygen, GameManager.instance.oxygenPerTurn < 0 ? "" : "+", GameManager.instance.oxygenPerTurn);
        string energy = string.Format("{0} ({1}{2})", GameManager.instance.currentEnergy, GameManager.instance.energyPerTurn < 0 ? "" : "+", GameManager.instance.energyPerTurn);

        oxygenValue.text = oxygen;
        energyValue.text = energy;
        materialsValue.text = materials;

        funBar.fillAmount = ((float)GameManager.instance.currentFun / (float)GameManager.instance.maxFun);
    }

    // called when the fun amount reaches the goal
    public void DisplayVictoryScreen(int currentTurn)
    {
        endScreenBG.gameObject.SetActive(true);
        endScreenBG.sprite = victorySprite;
        endText.text = ("You won in " + currentTurn + " turns !");
    }

    public void DisplayGameOverScreen()
    {
        endScreenBG.gameObject.SetActive(true);
        endText.text = ("You lost ! You run out of oxygen...");
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
}