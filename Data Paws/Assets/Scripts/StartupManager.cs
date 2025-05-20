using UnityEngine;

public class StartupManager : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}

