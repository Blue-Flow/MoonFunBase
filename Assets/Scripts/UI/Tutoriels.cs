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
    private GameObject currentPannelToDeactivate;
    private string cPTDName;

    private void Start()
    {
        SetTutorials();
    }
    private void SetTutorials()
    {
        // currentID = 0 so that it can get the intel on step 1 with GetNextStep and display it
        currentID = 0;
        GetNextStep();
        DisplayCurrentStep();
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
        if (cPTDName != "none")
        {
            // Gets the gameObject in scene that is wearing the same name as
            // the string that is referenced in the corresponding TutorielStep
            // (is there a way to reference directly the right gameObject ? --> Image doesn't work, gameObject neither except if it is a prefab...)
            currentPannelToDeactivate = GameObject.Find(cPTDName);
            currentPannelToDeactivate.SetActive(false);
        }
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
            if (currentStep.pannelToDeactivate != "none")
            {
                cPTDName = currentStep.pannelToDeactivate;
            }
            else cPTDName = "none";
        }
        else
        {
            nextStepButton.gameObject.SetActive(false);
        }
    }
}
