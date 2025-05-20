using UnityEngine;

// Handles transitioning to Level 2 when a button or trigger is activated
public class ToLevel2 : MonoBehaviour
{
    // Called to load the "Level 2" scene
    public void StartLevel2()
    {
        // Use the scene transition manager to load the scene with fade
        SceneTransitionManager.instance.LoadScene("Level 2");
    }
}