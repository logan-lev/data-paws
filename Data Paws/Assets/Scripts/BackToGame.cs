using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!string.IsNullOrEmpty(SceneTracker.PreviousSceneName))
            {
                SceneManager.LoadScene(SceneTracker.PreviousSceneName);
            }
            else
            {
                Debug.LogWarning("Previous scene name is not set!");
            }
        }
    }
}
