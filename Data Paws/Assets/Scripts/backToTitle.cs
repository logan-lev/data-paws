using UnityEngine;
using UnityEngine.SceneManagement;

public class backToTitle : MonoBehaviour
{
    public void BackToTitleScreen()
    {
        SceneTransitionManager.instance.LoadScene("Main Title");
    }
}
