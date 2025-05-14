using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLevel1 : MonoBehaviour
{
    public void StartLevel1()
    {
        SceneTransitionManager.instance.LoadScene("Level 1");
    }
}
