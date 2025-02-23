using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer; // Assign your AudioMixer in the Inspector

    [Header("Volume Levels")]
    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    private bool isMuted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadSettings(); // Load saved settings at start
    }

    //Adjust Master Volume
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        SaveSettings();
    }

    // 🎵 Adjust Music Volume
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        SaveSettings();
    }

    //djust SFX Volume
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        SaveSettings();
    }

    //Mute/Unmute All Sound
    public void ToggleMute()
    {
        isMuted = !isMuted;
        float muteValue = isMuted ? -80f : Mathf.Log10(masterVolume) * 20;
        audioMixer.SetFloat("MasterVolume", muteValue);
        SaveSettings();
    }

    //Save Settings
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Load Settings
    private void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        audioMixer.SetFloat("MasterVolume", isMuted ? -80f : Mathf.Log10(masterVolume) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }
}
