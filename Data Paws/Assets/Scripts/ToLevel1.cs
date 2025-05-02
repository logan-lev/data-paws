using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace to access scene management functions

public class ToLevel1 : MonoBehaviour
{
    public void StartLevel1()
    {
        SceneManager.LoadScene("Level 1"); // Replace "Level1" with the name of your level 1 scene
    }
}
