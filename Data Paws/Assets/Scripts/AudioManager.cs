using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Device;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public AudioMixer audioMixer;
    public Slider musicSlider;

    void Start()
    {
        if (musicSlider != null)
        {
            float value;
            audioMixer.GetFloat("MusicVolume", out value);
            musicSlider.value = Mathf.Pow(10, value / 20f); // convert dB to 0–1 scale
        }
    }


    public void SetMusicVolume()
    {
        float musicVol = musicSlider.value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVol) * 20); // Convert 0–1 scale to dB
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
}

