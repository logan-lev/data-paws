using UnityEngine;

// Handles transitioning to Level 1 when a button or trigger is activated
public class ToLevel1 : MonoBehaviour
{
    // Called to load the "Level 1" scene
    public void StartLevel1()
    {
        // Use the scene transition manager to load the scene with fade
        SceneTransitionManager.instance.LoadScene("Level 1");
    }
}