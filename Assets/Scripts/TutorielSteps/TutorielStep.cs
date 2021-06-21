using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorielStepN°X", menuName = "ScriptableObjects/Tutoriel")]
public class TutorielStep : ScriptableObject
{
    public int ID;
    public string textToDisplay;
    public Sprite imageToDisplay;
    public string pannelToDeactivate;
}
