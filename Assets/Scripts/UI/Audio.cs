using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip victoryTheme;
    [SerializeField] AudioClip defeatTheme;

    [SerializeField] AudioClip[] gHConstructionSounds;
    [SerializeField] AudioClip[] sPConstructionSounds;
    [SerializeField] AudioClip[] fHConstructionSounds;

    [SerializeField] AudioMixer audioMixer;

    public static Audio instance;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        EventsSubscribe();
    }
    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("volume");
        audioMixer.SetFloat("volume", volume);
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
    private void PlayEndTheme(bool victory, int turnNumber, ResourceType resourceType)
    {
        if (victory)
        {
            audioSource.clip = victoryTheme;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = defeatTheme;
            audioSource.Play();
        }
    }

    #region Events
    private void EventsSubscribe()
    {
        EventHandler.OnEndGame += PlayEndTheme;
        //EventHandler.OnEndTurn += UpdateValueText;
        //EventHandler.BuildCanceled +=
    }

    private void EventsClear()
    {
        EventHandler.OnEndGame -= PlayEndTheme;
    }
    #endregion

}
