using UnityEngine;
using UnityEngine.SceneManagement;
public class ToSetting : MonoBehaviour
{
    public void ToSettingScreen()
    {
        SceneTransitionManager.instance.LoadScene("Settings");
    }
}
