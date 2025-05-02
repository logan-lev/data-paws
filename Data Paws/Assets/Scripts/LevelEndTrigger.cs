using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    public FadeController fadeController;
    public string nextSceneName = "";
    public bool onlyFade = false;

    private void Awake()
    {
        // If fadeController not assigned in Inspector, find the persistent one
        if (fadeController == null)
        {
            fadeController = FindFirstObjectByType<FadeController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && fadeController != null)
        {
            if (onlyFade)
            {
                fadeController.FadeToBlackOnly();
            }
            else
            {
                fadeController.FadeToScene(nextSceneName);
            }
        }
    }
}

