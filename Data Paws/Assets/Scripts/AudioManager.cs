using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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

    void Update()
    {
        if (musicSlider.value == 0)
        {
            audioMixer.SetFloat("MusicVolume", -80f); // Mute
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20); // Convert 0–1 scale to dB
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

