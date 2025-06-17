using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject settingsPanel;

    bool isPaused;
    bool isSettingsOpen;

    private void Start()
    {
        isPaused = false;
        isSettingsOpen = false;
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    public void TogglePause()
    {
        if(!isSettingsOpen){
            isPaused = !isPaused;
            pauseMenuUI.SetActive(isPaused);
            pauseButton.SetActive(!isPaused);
            if (isPaused)
            {
                Time.timeScale = 0f; // Pause the game
            }
            else
            {
                Time.timeScale = 1f; // Resume the game
            }
        }
    }

    public void ToggleSettings()
    {
        isSettingsOpen = !isSettingsOpen;
        settingsPanel.SetActive(isSettingsOpen);
    }

    public void BackToMenu()
    {
        SaveSystem.currentSave.Save();
        SceneManager.LoadScene("MainMenu");
    }
}
