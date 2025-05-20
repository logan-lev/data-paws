using UnityEngine;
using UnityEngine.SceneManagement;

// Handles UI button interaction on the title screen
public class TitleSceneController : MonoBehaviour
{
    // Called when the "Start Game" button is pressed
    public void StartGame()
    {
        // Use the scene transition manager to load the "Tutorial" scene
        SceneTransitionManager.instance.LoadScene("Tutorial");
    }
}