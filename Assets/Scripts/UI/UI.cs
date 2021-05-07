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
    [SerializeField] Image funBar;
    [SerializeField] TextMeshProUGUI curTurnText;
    [SerializeField] TextMeshProUGUI baseName;

    [SerializeField] GameObject oxygenBuildingButtonHighlight;
    [SerializeField] GameObject energyBuildingButtonHighlight;
    [SerializeField] GameObject funBuildingButtonHighlight;

    [SerializeField] Image deathScreenBG;
    [SerializeField] TextMeshProUGUI deathEndText;
    [SerializeField] Image victoryScreenBG;
    [SerializeField] TextMeshProUGUI victoryEndText;

    [SerializeField] Image notificationBG;
    [SerializeField] TextMeshProUGUI notificationText;

    public static UI instance;

    void Awake ()
    {
        instance = this;
    }

    void Start ()
    {
        GetBaseName();
    }

    private void GetBaseName()
    {
        baseName.text = PlayerPrefs.GetString("baseName", "MoonFunBase");
    }

    public void ToggleBuildingButtonHighlight(BuildingType buildingType, bool toggle)
    {
        switch (buildingType)
        {
            case BuildingType.Fun:
                funBuildingButtonHighlight.SetActive(toggle);
                break;
            case BuildingType.SolarPanel:
                energyBuildingButtonHighlight.SetActive(toggle);
                break;
            case BuildingType.Greenhouse:
                oxygenBuildingButtonHighlight.SetActive(toggle);
                break;
        }
    }

    // called when the "End Turn" button is pressed
    public void UpdateTurnText(int currentTurn)
    {
        curTurnText.text = "Turn " + currentTurn;
    }

    // called when we place a building or the turn has ended
    public void UpdateFunBarAmount (float funBarAmount)
    {
        funBar.fillAmount = funBarAmount;
    }

    // called when the fun amount reaches the goal
    public void DisplayVictoryScreen(int currentTurn)
    {
        victoryScreenBG.gameObject.SetActive(true);
        victoryScreenBG.gameObject.GetComponent<AudioSource>().Play();
        victoryEndText.text = ("You won in " + currentTurn + " turns !");
    }

    public void DisplayGameOverScreen(ResourceType resource)
    {
        deathScreenBG.gameObject.SetActive(true);
        deathScreenBG.gameObject.GetComponent<AudioSource>().Play();
        if (resource == ResourceType.Oxygen)
        deathEndText.text = ("You lost ! You ran out of oxygen..." +"\n" +"Your people slowly died of suffocation");
        if(resource == ResourceType.Energy)
        deathEndText.text = ("You lost ! You ran out of energy..." +"\n" +"Your life support systems shut off and your people froze to death");
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

    public void UpdateFunText(int currentFun, int funPerTurn)
    {
        string fun = string.Format("{0} ({1}{2})", currentFun, funPerTurn < 0 ? "" : "+", funPerTurn);
        funValue.text = fun;
    }
    public void UpdateMaterialsText(int currentMaterials, int materialsPerTurn)
    {
        string materials = string.Format("{0} ({1}{2})", currentMaterials, materialsPerTurn < 0 ? "" : "+", materialsPerTurn);
        materialsValue.text = materials;
    }

    internal void UpdateOxygenText(int currentOxygen, int oxygenPerTurn)
    {
        string oxygen = string.Format("{0} ({1}{2})", currentOxygen, oxygenPerTurn < 0 ? "" : "+", oxygenPerTurn);
        oxygenValue.text = oxygen;
    }

    internal void UpdateEnergyText(int currentEnergy, int energyPerTurn)
    {
        string energy = string.Format("{0} ({1}{2})", currentEnergy, energyPerTurn < 0 ? "" : "+", energyPerTurn);
        energyValue.text = energy;
    }
}