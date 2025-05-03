using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;
    public Image fadeImage;
    public float fadeDuration = 1f;
    public TMPro.TMP_Text youDidItText;

    public float messageFadeDelay = 0.5f;
    public float messageFadeDuration = 1f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        StartCoroutine(Fade(1, 0)); // Start by fading from black to clear
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
{
    // Fade to black first
    yield return Fade(0, 1); // From transparent to black

    // Wait a single frame to ensure the screen is black before switching
    yield return null;

    // Load new scene (OnSceneLoaded handles the fade-in)
    SceneManager.LoadScene(sceneName);
}

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(Fade(1, 0)); // Fade back in
    }

    public IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(c.r, c.g, c.b, endAlpha);

// Auto-show message when fading to black in Level 2
if (SceneManager.GetActiveScene().name == "Level 2" && startAlpha == 0f && endAlpha == 1f)
{
    StartCoroutine(ShowYouDidItMessage());
}
    }
    public void LoadPauseAdditive(string pauseSceneName)
{
    StartCoroutine(LoadPauseSceneAdditive(pauseSceneName));
}

private IEnumerator LoadPauseSceneAdditive(string sceneName)
{
    yield return Fade(0f, 1f); // Fade to black

    yield return new WaitForSeconds(0.3f);

    SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

    yield return Fade(1f, 0f); // Fade back in
}
public IEnumerator ShowYouDidItMessage()
{
    yield return new WaitForSeconds(messageFadeDelay);

    if (youDidItText != null)
    {
        youDidItText.gameObject.SetActive(true);

        Color c = youDidItText.color;
        float elapsed = 0f;

        while (elapsed < messageFadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / messageFadeDuration);
            youDidItText.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        youDidItText.color = new Color(c.r, c.g, c.b, 1f);
    }
}


}

