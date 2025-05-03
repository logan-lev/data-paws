using UnityEngine;
using UnityEngine.SceneManagement;

public class ToKeysAdjMenu : MonoBehaviour
{
    public void LoadKeysAdjMenu()
    {
        SceneTransitionManager.instance.LoadScene("KeyboardAdj");
    }
}
