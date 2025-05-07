using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    public void StartGame()
    {
        SceneTransitionManager.instance.LoadScene("Tutorial");
    }
}
