using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] AudioClip[] gHConstructionSounds;
    [SerializeField] AudioClip[] sPConstructionSounds;
    [SerializeField] AudioClip[] fHConstructionSounds;


    public static UI instance;

    void Awake ()
    {
        instance = this;
    }

    void Start ()
    {
        curTurnText.text = "Turn " + GameManager.instance.currentTurn;
        audioSource = GetComponent<AudioSource>();
        baseName.text = PlayerPrefs.GetString("baseName", "MoonFunBase");
    }
    public void PlayConstructionSound(BuildingType buildingType)
    {
        //play building sound depending on the constructed building
        switch (buildingType)
        {
            case (BuildingType.Greenhouse):
                AudioClip gHClip = gHConstructionSounds[Random.Range(0, gHConstructionSounds.Length)];
                audioSource.PlayOneShot(gHClip);
                break;
            case (BuildingType.Fun):
                AudioClip fHClip = fHConstructionSounds[Random.Range(0, fHConstructionSounds.Length)];
                audioSource.PlayOneShot(fHClip);
                break;
            case (BuildingType.SolarPanel):
                AudioClip sPClip = sPConstructionSounds[Random.Range(0, sPConstructionSounds.Length)];
                audioSource.PlayOneShot(sPClip);
                break;
        }
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
        string fun = string.Format("{0} ({1}{2})", GameManager.instance.currentFun, GameManager.instance.funPerTurn < 0 ? "" : "+", GameManager.instance.funPerTurn);

        oxygenValue.text = oxygen;
        energyValue.text = energy;
        materialsValue.text = materials;
        funValue.text = fun;

        funBar.fillAmount = ((float)GameManager.instance.currentFun / (float)GameManager.instance.maxFun);
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
}