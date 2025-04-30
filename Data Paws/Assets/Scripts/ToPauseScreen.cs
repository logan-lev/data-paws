using UnityEngine;
using UnityEngine.SceneManagement;

public class ToPauseScreen : MonoBehaviour
{
    public void LoadPauseScreen()
    {
        SceneManager.LoadScene("PauseScreen");
    }
}
