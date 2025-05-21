using UnityEngine;

public class StartupManager : MonoBehaviour
{
    private static bool prefsResetThisSession = false;

    void Awake()
    {
        if (!prefsResetThisSession)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            prefsResetThisSession = true;
        }
    }
}
