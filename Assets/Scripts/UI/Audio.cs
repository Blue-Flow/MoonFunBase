using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip victoryTheme;
    [SerializeField] AudioClip defeatTheme;
    [SerializeField] AudioClip mainTheme;

    [SerializeField] AudioClip[] gHConstructionSounds;
    [SerializeField] AudioClip[] sPConstructionSounds;
    [SerializeField] AudioClip[] fHConstructionSounds;
    [SerializeField] AudioClip[] endTurnSounds;
    [SerializeField] AudioClip[] errorSounds;

    [SerializeField] AudioMixer audioMixer;

    public static Audio instance;

    private void Awake()
    {
        int audioCount = FindObjectsOfType<Audio>().Length;
        if (audioCount > 1) { Destroy(gameObject); }
        else DontDestroyOnLoad(gameObject);

        instance = this;

        audioSource = GetComponent<AudioSource>();

        EventsSubscribe();
    }
    private void StartAudio()
    {
        audioSource.clip = mainTheme;
        audioSource.Play();
    }
    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("volume");
        audioMixer.SetFloat("volume", volume);
    }

    private void PlayConstructionSound(BuildingPreset buildingPreset, TileType tileType, Vector2 tilePosition)
    {
        //play building sound depending on the constructed building
        switch (buildingPreset.buildingType)
        {
            case (BuildingType.Oxygen):
                AudioClip gHClip = gHConstructionSounds[Random.Range(0, gHConstructionSounds.Length)];
                audioSource.PlayOneShot(gHClip);
                break;
            case (BuildingType.Fun):
                AudioClip fHClip = fHConstructionSounds[Random.Range(0, fHConstructionSounds.Length)];
                audioSource.PlayOneShot(fHClip);
                break;
            case (BuildingType.Energy):
                AudioClip sPClip = sPConstructionSounds[Random.Range(0, sPConstructionSounds.Length)];
                audioSource.PlayOneShot(sPClip);
                break;
        }
    }
    private void PlayEndTurnSound()
    {
        AudioClip endTurnClip = endTurnSounds[Random.Range(0, endTurnSounds.Length)];
        audioSource.PlayOneShot(endTurnClip);
    }
    private void PlayErrorSound(int errorNumber)
    {
        AudioClip errorClip = errorSounds[Random.Range(0, errorSounds.Length)];
        audioSource.PlayOneShot(errorClip);
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
    private void ClearAudio()
    {
        audioSource.Stop();
    }

    #region Events
    private void EventsSubscribe()
    {
        EventHandler.OnEndGame += PlayEndTheme;
        EventHandler.OnBuildCompleted += PlayConstructionSound;
        EventHandler.OnEndTurn += PlayEndTurnSound;
        EventHandler.OnError += PlayErrorSound;
        EventHandler.OnStartGame += StartAudio;
        EventHandler.OnClearGame += ClearAudio;
    }

    #endregion

}
