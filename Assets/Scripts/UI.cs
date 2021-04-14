using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public GameObject buildingButtons;

    public TextMeshProUGUI oxygenValue;
    public TextMeshProUGUI energyValue;
    public TextMeshProUGUI materialsValue;
    public Image funBar;

    public TextMeshProUGUI curTurnText;

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

    // called when we select a building (disables the buttons)
    public void ToggleBuildingButtons (bool toggle)
    {
        buildingButtons.SetActive(toggle);
    }

    // called when we click the solar panel button
    public void OnClickSolarPanelButton ()
    {
        GameManager.instance.SetPlacingBuilding(BuildingType.SolarPanel);
        ToggleBuildingButtons(false);
    }

    // called when we click the greenhouse button
    public void OnClickGreenhouseButton()
    {
        GameManager.instance.SetPlacingBuilding(BuildingType.Greenhouse);
        ToggleBuildingButtons(false);
    }

    // called when we click the mine button
    public void OnClickFunhouseButton()
    {
        GameManager.instance.SetPlacingBuilding(BuildingType.Fun);
        ToggleBuildingButtons(false);
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
}