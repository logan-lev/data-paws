using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BackToGame : MonoBehaviour
{
    public void ResumeGame()
    {
        StartCoroutine(ResumeFromPause());
    }

    private IEnumerator ResumeFromPause()
    {
        // Optional: Freeze input or play sound here

        yield return SceneTransitionManager.instance.Fade(0f, 1f); // Fade to black
        yield return new WaitForSeconds(0.3f); // Optional pause for effect

        Scene pauseScene = SceneManager.GetSceneByName("PauseScreen");
        if (pauseScene.IsValid() && pauseScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(pauseScene); // Unload PauseScreen only
        }

        yield return SceneTransitionManager.instance.Fade(1f, 0f); // Fade back in
    }
}
