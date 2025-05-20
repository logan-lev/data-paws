using UnityEngine;

// This script handles transitioning back to the main title screen.
public class backToTitle : MonoBehaviour
{
    // Uses the SceneTransitionManager to load the "Main Title" scene
    public void BackToTitleScreen()
    {
        SceneTransitionManager.instance.LoadScene("Main Title");
    }
}
