using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject settingsPanel;

    public Slider musicSlider;
    public Slider sfxSlider;

    private float musicVolume;
    private float sfxVolume;

    void Start()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }

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

        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void Back()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}