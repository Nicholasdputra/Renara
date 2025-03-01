using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Button backButton;
    private Resolution[] supportedResolutions = {
        new Resolution {width = 640, height = 360},
        new Resolution {width = 854, height = 480},
        new Resolution {width = 960, height = 540},
        new Resolution {width = 1024, height = 576},
        new Resolution {width = 1280, height = 720},
        new Resolution {width = 1366, height = 768},
        new Resolution {width = 1600, height = 900},
        new Resolution {width = 1920, height = 1080},
        new Resolution {width = 2560, height = 1440},
        new Resolution {width = 3200, height = 1800},
        new Resolution {width = 3840, height = 2160},
    };
    Resolution[] nativeResolutions;
    List<Resolution> finalResolutions;

    void Start()
    {
        
        nativeResolutions = Screen.resolutions;
        finalResolutions = new List<Resolution>();
        // for(int i = 0; i < nativeResolutions.Length; i++)
        // {
        //     Debug.Log(nativeResolutions[i].width + " x " + nativeResolutions[i].height);
        // }
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < nativeResolutions.Length; i++)
        {
            for (int j = 0; j < supportedResolutions.Length; j++)
            {
                if (nativeResolutions[i].width == supportedResolutions[j].width && nativeResolutions[i].height == supportedResolutions[j].height)
                {
                    if(!finalResolutions.Contains(supportedResolutions[j]))
                    {
                        string resolutionString = supportedResolutions[j].width + " x " + supportedResolutions[j].height;
                        finalResolutions.Add(supportedResolutions[j]);
                        options.Add(resolutionString);

                        if (supportedResolutions[j].width == Screen.currentResolution.width && supportedResolutions[j].height == Screen.currentResolution.height)
                        {
                            currentResolutionIndex = options.Count - 1;
                        }
                    }
                }
            }
        }
        // for(int i = 0; i < finalResolutions.Count; i++)
        // {
        //     Debug.Log(finalResolutions[i].width + " x " + finalResolutions[i].height);
        // }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        LoadSettings();

        // Add listeners
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        backButton.onClick.AddListener(() => { gameObject.SetActive(false); });
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = finalResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // Save resolution setting
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        // Save fullscreen setting
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        Debug.Log("Setting volume to " + volume);

        // Save volume setting
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        // Load resolution setting
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
            resolutionDropdown.value = resolutionIndex;
            resolutionDropdown.RefreshShownValue();
            SetResolution(resolutionIndex);
        }
        else
        {
            resolutionDropdown.value = finalResolutions.Count - 1;
            resolutionDropdown.RefreshShownValue();
            SetResolution(finalResolutions.Count - 1);
            PlayerPrefs.SetInt("ResolutionIndex", finalResolutions.Count - 1);
        }

        // Load fullscreen setting
        if (PlayerPrefs.HasKey("Fullscreen")){
            bool isFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1;
            fullscreenToggle.isOn = isFullscreen;
            Screen.fullScreen = isFullscreen;
        }
        else{
            //default
            fullscreenToggle.isOn = Screen.fullScreen;
            PlayerPrefs.SetInt("Fullscreen", 1);
        }

        // Load volume setting
        if (PlayerPrefs.HasKey("Volume")){
            float volume = PlayerPrefs.GetFloat("Volume");
            volumeSlider.value = volume;
            AudioListener.volume = volume;
        }
        else{
            volumeSlider.value = AudioListener.volume;
            PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        }

        PlayerPrefs.Save();
    }
}
