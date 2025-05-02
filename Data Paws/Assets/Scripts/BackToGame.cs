using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace to access scene management functions
public class BackToGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.P))
     {
        // Load the game scene when the "P" key is pressed
        SceneManager.LoadScene("Tutorial"); // Replace "GameScene" with the name of your game scene
     }   
    }
}
