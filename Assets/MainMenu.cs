using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Main Menu manager. Handles panel switching, settings, and scene navigation.
/// All UI layout and styling is done in the MainMenu scene via Inspector.
/// This script only handles LOGIC.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Audio")]
    public AudioSource backgroundMusic;

    private float musicVolume;
    private float sfxVolume;

    void Start()
    {
        // Load saved settings
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        }

        if (backgroundMusic != null)
        {
            backgroundMusic.volume = musicVolume;
            if (!backgroundMusic.isPlaying)
            {
                backgroundMusic.Play();
            }
        }

        if (mainPanel != null) mainPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // ===== MAIN MENU =====

    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    public void OpenSettings()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    // ===== SETTINGS =====

    public void OnMusicChanged(float value)
    {
        musicVolume = value;
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = musicVolume;
        }
    }

    public void OnSFXChanged(float value)
    {
        sfxVolume = value;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();

        Debug.Log("Settings Saved");

        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainPanel != null) mainPanel.SetActive(true);
    }

    public void Back()
    {
        // Kembalikan slider ke nilai terakhir yang tersimpan
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        if (musicSlider != null) musicSlider.value = savedVolume;
        if (sfxSlider != null) sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (backgroundMusic != null)
        {
            backgroundMusic.volume = savedVolume;
        }

        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainPanel != null) mainPanel.SetActive(true);
    }
}
