using UnityEngine;

// Handles transitioning to the level selection screen
public class ToLevels : MonoBehaviour
{
    // Called to load the "Levels Page" scene
    public void LoadLevelsScene()
    {
        // Use the scene transition manager to load the scene with fade
        SceneTransitionManager.instance.LoadScene("Levels Page");
    }
}