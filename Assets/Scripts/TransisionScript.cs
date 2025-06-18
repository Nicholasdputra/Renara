using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class TransisionScript : MonoBehaviour
{
    public static TransisionScript instance;
    Image fadeImage;
    AudioSource audioSource;
    [SerializeField] AudioClip transitionSound;
    bool isTransitioning = false;

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
