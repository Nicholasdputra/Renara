using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransisionScript : MonoBehaviour
{
    public static TransisionScript instance;
    Image fadeImage;
    AudioSource audioSource;
    [SerializeField] AudioClip transitionSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            fadeImage = GetComponent<Image>();
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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

        // Fade in
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(1 - (elapsedTime / fadeDuration)));
            yield return null;
        }
    }
}
