using UnityEngine;

public class ApplyGlobalVolume : MonoBehaviour
{
    void Start()
    {
        var audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.volume = AudioManager.globalVolume;
        }
    }
}

