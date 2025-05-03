using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    public string nextSceneName = "";
    public bool onlyFade = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && SceneTransitionManager.instance != null)
        {
            if (onlyFade)
            {
                // Just fade to black and stay in the current scene
                SceneTransitionManager.instance.StartCoroutine(
                    SceneTransitionManager.instance.Fade(0f, 1f)
                );
            }
            else if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneTransitionManager.instance.LoadScene(nextSceneName);
            }
        }
    }
}

