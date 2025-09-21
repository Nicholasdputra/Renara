using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] Button quitButton;

    void Start()
    {
        Time.timeScale = 1f;
        playButton.onClick.AddListener(Play);
        settingsButton.onClick.AddListener(Settings);
        quitButton.onClick.AddListener(Quit);
        settingsPanel.GetComponent<SettingsManager>().LoadSettings();
    }

    void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Overworld");
    }

    void Settings()
    {
        settingsPanel.SetActive(true);
    }

    void Quit()
    {
        Application.Quit();
    }
}
