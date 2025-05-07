using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseScreenManager : MonoBehaviour
{
    public void ResumeGame()
    {
        StartCoroutine(UnloadPauseScene());
    }

    private IEnumerator UnloadPauseScene()
    {
        yield return SceneTransitionManager.instance.Fade(0f, 1f);
        yield return new WaitForSeconds(0.3f);
        SceneManager.UnloadSceneAsync("Pause Screen");
        yield return SceneTransitionManager.instance.Fade(1f, 0f);
    }
}

