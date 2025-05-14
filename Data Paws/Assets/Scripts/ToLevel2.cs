using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLevel2 : MonoBehaviour
{
    public void StartLevel2()
    {
        SceneTransitionManager.instance.LoadScene("Level 2");
    }
}
