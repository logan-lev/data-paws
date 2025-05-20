using UnityEngine;

// This script handles the end-of-level trigger,
// either fading out or transitioning to the next scene when the player enters.
public class LevelEndTrigger : MonoBehaviour
{
    // Name of the next scene to load
    public string nextSceneName = "";

    // If true, only fade the screen to black without changing scenes
    public bool onlyFade = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player and the transition manager exists
        if (other.CompareTag("Player") && SceneTransitionManager.instance != null)
        {
            // If onlyFade is true, perform a fade-out without changing the scene
            if (onlyFade)
            {
                // Start a coroutine to fade the screen to black
                SceneTransitionManager.instance.StartCoroutine(
                    SceneTransitionManager.instance.Fade(0f, 1f)
                );
            }
            // If a scene name is provided, load the next scene
            else if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneTransitionManager.instance.LoadScene(nextSceneName);
            }
        }
    }
}