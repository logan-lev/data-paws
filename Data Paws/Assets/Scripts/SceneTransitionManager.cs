using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// Manages scene transitions with fade effects and optional messages
public class SceneTransitionManager : MonoBehaviour
{
    // Singleton instance for global access
    public static SceneTransitionManager instance;

    // UI image used for fade-in/out effect
    public Image fadeImage;

    // Duration of the fade animation
    public float fadeDuration = 1f;

    // Timing for message display
    public float messageFadeDelay = 0.5f;
    public float messageFadeDuration = 1f;

    private void Awake()
    {
        // Enforce singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Persist across scene loads
        DontDestroyOnLoad(gameObject);

        // Subscribe to scene load event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Begin by fading from black to clear
        StartCoroutine(Fade(1, 0));
    }

    // Public method to initiate a scene transition
    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    // Handles fading out, loading a new scene, then fading in
    private IEnumerator Transition(string sceneName)
    {
        // Fade to black first
        yield return Fade(0, 1);

        // Wait one frame before switching scenes
        yield return null;

        // Load the new scene (fade-in will be triggered on load)
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Fade from black to clear after scene loads
        StartCoroutine(Fade(1, 0));
    }

    // Coroutine to fade the screen from one alpha to another
    public IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        // Interpolate alpha over time
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        // Finalize the fade
        fadeImage.color = new Color(c.r, c.g, c.b, endAlpha);
    }

    // Public method to load a pause menu additively with fade
    public void LoadPauseAdditive(string pauseSceneName)
    {
        StartCoroutine(LoadPauseSceneAdditive(pauseSceneName));
    }

    // Coroutine to fade, load a scene additively, then fade in
    private IEnumerator LoadPauseSceneAdditive(string sceneName)
    {
        yield return Fade(0f, 1f); // Fade to black

        yield return new WaitForSeconds(0.3f); // Small delay

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive); // Load pause scene

        yield return Fade(1f, 0f); // Fade back in
    }
}