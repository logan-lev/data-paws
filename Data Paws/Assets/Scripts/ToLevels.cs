using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLevels : MonoBehaviour
{
    public void LoadLevelsScene()
    {
        SceneTransitionManager.instance.LoadScene("Levels Page");
    }
}
