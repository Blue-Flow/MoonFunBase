using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] AudioClip[] gHConstructionSounds;
    [SerializeField] AudioClip[] sPConstructionSounds;
    [SerializeField] AudioClip[] fHConstructionSounds;

    [SerializeField] AudioMixer audioMixer;

    public static Audio instance;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
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

}
