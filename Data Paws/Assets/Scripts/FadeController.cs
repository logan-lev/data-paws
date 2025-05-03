using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;


public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    private static FadeController instance;
    public TMP_Text endMessageText;


    private void Awake()
    {
        // Ensure one instance persists across scenes
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 1); // start fully black

        // Hook into scene load event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Fade in at the start of the first scene
        StartCoroutine(FadeIn());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        Color start = new Color(0, 0, 0, 1); // black
        Color end = new Color(0, 0, 0, 0);   // transparent

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = Color.Lerp(start, end, t);
            yield return null;
        }

        fadeImage.color = end;
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        float timer = 0f;
        Color start = fadeImage.color;
        Color end = new Color(0, 0, 0, 1); // black

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = Color.Lerp(start, end, t);
            yield return null;
        }

        SceneTransitionManager.instance.LoadScene(sceneName);
    }
    public void FadeToBlackOnly()
{
    StartCoroutine(FadeOut());
}

private IEnumerator FadeOut()
{
    float timer = 0f;
    Color startColor = fadeImage.color;
    Color endColor = new Color(0, 0, 0, 1); // black

    Color textStartColor = endMessageText != null ? new Color(endMessageText.color.r, endMessageText.color.g, endMessageText.color.b, 0f) : Color.clear;
    Color textEndColor = endMessageText != null ? new Color(endMessageText.color.r, endMessageText.color.g, endMessageText.color.b, 1f) : Color.clear;

    while (timer < fadeDuration)
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / fadeDuration);

        fadeImage.color = Color.Lerp(startColor, endColor, t);

        if (endMessageText != null)
        {
            endMessageText.color = Color.Lerp(textStartColor, textEndColor, t);
        }

        yield return null;
    }

    
    fadeImage.color = endColor;
    if (endMessageText != null)
        endMessageText.color = textEndColor;
}
}