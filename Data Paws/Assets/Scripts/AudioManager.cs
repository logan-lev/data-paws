using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static float globalVolume = 1f;

    public Slider musicSlider;
    public AudioSource audioSource; // optional if controlling a local source

    private const string VolumeKey = "GlobalVolume";

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Load volume from PlayerPrefs or use default
        globalVolume = PlayerPrefs.HasKey(VolumeKey) ? PlayerPrefs.GetFloat(VolumeKey) : 1f;

        if (musicSlider != null)
            musicSlider.value = globalVolume;

        SetMusicVolume();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetMusicVolume()
    {
        globalVolume = musicSlider.value;

        // Save volume
        PlayerPrefs.SetFloat(VolumeKey, globalVolume);
        PlayerPrefs.Save();

        // Update all current AudioSources
        foreach (AudioSource src in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            if (src != null && src.enabled)
                src.volume = globalVolume;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Main Title" && scene.name != "Levels Page")
        {
            if (TryGetComponent<AudioSource>(out var src))
                src.Stop();

            Destroy(gameObject);
        }
    }
}