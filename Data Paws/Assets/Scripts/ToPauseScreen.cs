using UnityEngine;

public class ToPauseScreen : MonoBehaviour
{
    public void LoadPauseScreen()
    {
        SceneTransitionManager.instance.LoadPauseAdditive("PauseScreen");
    }
}

