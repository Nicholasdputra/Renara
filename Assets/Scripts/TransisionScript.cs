using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections.Generic;

public class TransisionScript : MonoBehaviour
{
    public static TransisionScript instance;
    Image fadeImage;
    AudioSource audioSource;
    [SerializeField] AudioClip transitionSound;
    bool isTransitioning = false;
    public GameObject[] overworldPlants;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            fadeImage = GetComponent<Image>();
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            isTransitioning = false;
        }
        else
        {
            Destroy(gameObject);
        }

        if(SceneManager.GetActiveScene().name.Equals("Overworld"))
        {
            overworldPlants = GameObject.FindGameObjectsWithTag("PlantToExtract");
            for (int i = 0; i < overworldPlants.Length; i++)
            {
                for(int j = 0; j < SaveSystem.currentSave.currentPlayerData.plantDataSO.plant.Length; j++)
                {
                    if (SaveSystem.currentSave.currentPlayerData.plantDataSO.plant[j].plantName == overworldPlants[i].name)
                    {
                        // Debug.Log("Plant found: " + overworldPlants[i].name);
                        // Debug.Log("Is unlocked: " + SaveSystem.currentSave.currentPlayerData.plantDataSO.plant[j].isUnlocked);
                        if (SaveSystem.currentSave.currentPlayerData.plantDataSO.plant[j].isUnlocked)
                        {
                            overworldPlants[i].SetActive(false);
                        }
                        else
                        {
                            overworldPlants[i].SetActive(true);
                        }
                        break;
                    }
                }
            }
        }
    }

    public static void Transision(string sceneName)
    {
        if (instance != null)
        {
            instance.StartCoroutine(instance.TransisionCoroutine(sceneName));
        }
    }

    IEnumerator TransisionCoroutine(string sceneName = "")
    {
        if (isTransitioning)
        {
            yield break; // Prevent multiple transitions at the same time
        }
        isTransitioning = true;
        if (!sceneName.Equals(""))
        {
            audioSource.PlayOneShot(transitionSound);
        }
        // Fade out
        fadeImage.color = new Color(0, 0, 0, 0);
        float fadeDuration = 1.3f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(elapsedTime / fadeDuration));
            yield return null;
        }

        // Load the scene
        if (!sceneName.Equals(""))
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }

        // Fade in
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(1 - (elapsedTime / fadeDuration)));
            yield return null;
        }
        isTransitioning = false;
    }
}
