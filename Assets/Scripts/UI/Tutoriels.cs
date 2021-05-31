using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Tutoriels : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textField;
    [SerializeField] Image imageField;
    [SerializeField] Button nextStepButton;
    [SerializeField] GameObject tutorialBG;

    [SerializeField] List<TutorielStep> tutorielsStepList;
    private int currentID = 0;
    private string currentText;
    private Sprite currentSprite;

    private void Awake()
    {
        int TutorielsCount = FindObjectsOfType<Tutoriels>().Length;
        if (TutorielsCount > 1) { Destroy(gameObject); }
        else DontDestroyOnLoad(gameObject);

        EventsSubscribe();
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("areTipsactive") != 2)
            EventHandler.SetTutorial();
    }
    private void SetTutorials()
    {
        currentID = 0;
        GetNextStep();
        DisplayCurrentStep();
        tutorialBG.SetActive(true);
    }
    public void ClickNextButton()
    {
        DisplayCurrentStep();
    }
    private void DisplayCurrentStep()
    {
        textField.text = currentText;
        if (currentSprite != null)
        {
            imageField.gameObject.SetActive(true);
            imageField.sprite = currentSprite;
        }
        else
            imageField.gameObject.SetActive(false);
        GetNextStep();
    }
    private void GetNextStep()
    {
        currentID += 1;
        TutorielStep currentStep = tutorielsStepList.Find(x => x.ID == currentID);
        if (currentStep != null)
        {
            currentSprite = currentStep.imageToDisplay;
            currentText = currentStep.textToDisplay;
        }
        else
        {
            nextStepButton.gameObject.SetActive(false);
        }
    }
    #region Events
    private void EventsSubscribe()
    {
        EventHandler.OnSetTutorial += SetTutorials;
    }
    #endregion 
}
