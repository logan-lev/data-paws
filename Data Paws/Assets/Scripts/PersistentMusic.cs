using UnityEngine;
using UnityEngine.SceneManagement;

// This script keeps background music persistent across scenes,
// but destroys it once the player enters gameplay scenes.
public class PersistentMusic : MonoBehaviour
{
    // Static instance to enforce singleton pattern
    private static PersistentMusic instance;

    private void Awake()
    {
        // If another instance already exists, destroy this one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the singleton instance and prevent destruction on load
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Destroy the music object when entering gameplay scenes
        if (scene.name != "Main Title" && scene.name != "Levels Page")
        {
            Destroy(gameObject);
        }
    }
}