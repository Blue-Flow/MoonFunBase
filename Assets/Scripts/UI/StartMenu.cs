using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class StartMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioMixer audioMixer;
    [Header("Personalization")]
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI baseNameText;
    private string baseName;


    private void Start()
    {
        baseNameText.text = PlayerPrefs.GetString("baseName", "MoonFunBase");
    }

    public void ReloadScene()
    {
        EventHandler.ClearGame();
        EventHandler.StartGame();
        EventHandler.ButtonClicked();
    }

    public void QuitGame()
    {
        EventHandler.ButtonClicked();
        Application.Quit();
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetBaseName()
    {
        baseName = inputField.text;
        baseNameText.text = baseName;
        PlayerPrefs.SetString("baseName", baseName);
    }

    public void LoadNextScene()
    {
        EventHandler.ButtonClicked();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
