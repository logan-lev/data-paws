using UnityEngine;
using UnityEngine.SceneManagement;

public class backToTitle : MonoBehaviour
{
    public void BackToTitleScreen()
    {
        SceneManager.LoadScene("Main Title");
    }
}
