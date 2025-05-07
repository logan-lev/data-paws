using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace to access scene management functions

public class ToLevel2 : MonoBehaviour
{
    public void StartLevel2()
    {
        SceneTransitionManager.instance.LoadScene("Level 2"); // Replace "Level2" with the name of your level 2 scene
    }
}
