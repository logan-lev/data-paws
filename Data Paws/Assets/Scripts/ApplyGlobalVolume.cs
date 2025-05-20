using UnityEngine;

// This script sets the AudioSource's volume based on a global volume setting.
public class ApplyGlobalVolume : MonoBehaviour
{
    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        var audio = GetComponent<AudioSource>();

        // If an AudioSource is found, set its volume to the global volume value
        if (audio != null)
        {
            // Set the volume using a value defined in AudioManager
            audio.volume = AudioManager.globalVolume;
        }
    }
}

