using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    // used to play the main theme
    private AudioSource audioSource;
    // used to add an ambiant level linked to the current fun value
    // && avoid implementing Wwise just to deal with it
    private AudioSource audioSourceGameManager;

    [SerializeField] AudioClip victoryTheme;
    [SerializeField] AudioClip defeatTheme;
    [SerializeField] AudioClip mainTheme;

    [SerializeField] AudioClip funCap0SFX;
    [SerializeField] AudioClip funCap1SFX;
    [SerializeField] AudioClip funCap2SFX;

    [SerializeField] AudioClip buttonSound;
    [SerializeField] AudioClip[] gHConstructionSounds;
    [SerializeField] AudioClip[] sPConstructionSounds;
    [SerializeField] AudioClip[] fHConstructionSounds;
    [SerializeField] AudioClip[] endTurnSounds;
    [SerializeField] AudioClip[] errorSounds;

    [SerializeField] AudioMixer audioMixer;

    private void Awake()
    {
        EventsSubscribe();
        audioSource = GetComponent<AudioSource>();
        audioSourceGameManager = FindObjectOfType<GameManager>().GetComponent<AudioSource>();
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
    private void PlayNextAmbiantSound()
    {
        if (audioSourceGameManager.clip == funCap1SFX)
        {
            audioSourceGameManager.clip = funCap2SFX;
            audioSourceGameManager.Play();
        }
        else
        {
            audioSourceGameManager.clip = funCap1SFX;
            audioSourceGameManager.Play();
        }
        Debug.Log("NextAmbiantSound");
        Debug.Log(audioSourceGameManager.isPlaying);
    }
    private void PlayButtonSound()
    {
        AudioSource.PlayClipAtPoint(buttonSound, Vector3.zero, 1.0f);
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
    #region Events
    private void EventsSubscribe()
    {
        EventHandler.OnEndGame += PlayEndTheme;
        EventHandler.OnBuildCompleted += PlayConstructionSound;
        EventHandler.OnEndTurn += PlayEndTurnSound;
        EventHandler.OnError += PlayErrorSound;
        EventHandler.OnStartGame += StartAudio;
        EventHandler.OnButtonClicked += PlayButtonSound;
        EventHandler.OnNewFunCapReached += PlayNextAmbiantSound;
    }

    private void OnDestroy()
    {
        EventHandler.OnEndGame -= PlayEndTheme;
        EventHandler.OnBuildCompleted -= PlayConstructionSound;
        EventHandler.OnEndTurn -= PlayEndTurnSound;
        EventHandler.OnError -= PlayErrorSound;
        EventHandler.OnStartGame -= StartAudio;
        EventHandler.OnButtonClicked -= PlayButtonSound;
        EventHandler.OnNewFunCapReached -= PlayNextAmbiantSound;
    }
    #endregion
}
