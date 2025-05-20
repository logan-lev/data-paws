using UnityEngine;

// Deletes all saved player preferences when the game starts
public class StartupManager : MonoBehaviour
{
    void Start()
    {
        // Clear all stored PlayerPrefs data
        PlayerPrefs.DeleteAll();

        // Force PlayerPrefs to write changes to disk
        PlayerPrefs.Save();
    }
}