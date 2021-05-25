using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorielStepN°X", menuName = "ScriptableObjects/Tutoriel")]
public class TutorielStep : ScriptableObject
{
    public int ID;
    public string textToDisplay;
    public Sprite imageToDisplay;
}
