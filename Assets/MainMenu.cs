using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private float musicVolume;
    private float sfxVolume;

    void Start()
    {
        // Load setting yang tersimpan
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
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
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // ===== SETTINGS =====

    public void OnMusicChanged(float value)
    {
        musicVolume = value;
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

        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void Back()
    {
        // Kembalikan slider ke nilai terakhir yang tersimpan
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
