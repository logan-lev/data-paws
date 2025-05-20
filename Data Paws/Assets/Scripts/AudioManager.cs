using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This script manages the global audio volume and links it to a UI slider.
public class AudioManager : MonoBehaviour
{
    // Singleton instance to prevent duplicates
    private static AudioManager instance;

    // Shared volume level across all AudioSources
    public static float globalVolume = 1f;

    // Flag to ensure volume is loaded once
    private static bool volumeInitialized = false;

    // UI Slider for controlling volume
    public Slider musicSlider;

    // PlayerPrefs key for saving volume
    private const string VolumeKey = "GlobalVolume";

    void Awake()
    {
        // Enforce singleton pattern: destroy duplicates
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Persist across scene loads
        DontDestroyOnLoad(gameObject);

        // Load saved volume only once
        if (!volumeInitialized)
        {
            if (PlayerPrefs.HasKey(VolumeKey))
                globalVolume = PlayerPrefs.GetFloat(VolumeKey);
            else
                globalVolume = 1f;

            volumeInitialized = true;
        }
    }

    void Start()
    {
        // Try to find and link the volume slider
        SetupSlider();

        // Apply current global volume
        SetMusicVolume();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Tries to find the music slider in the scene and hook up the volume control
    private void SetupSlider()
    {
        // Find slider in scene if not assigned
        if (musicSlider == null)
        {
            musicSlider = GameObject.FindWithTag("MusicSlider")?.GetComponent<Slider>();
        }

        // Set slider value and listener if found
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.value = globalVolume;
            musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called when the slider value changes or at startup
    public void SetMusicVolume()
    {
        // Update global volume from slider
        if (musicSlider != null)
            globalVolume = musicSlider.value;

        // Save the volume to disk
        PlayerPrefs.SetFloat(VolumeKey, globalVolume);
        PlayerPrefs.Save();

        // Apply volume to all active AudioSources in the scene
        foreach (AudioSource src in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            if (src != null && src.enabled)
                src.volume = globalVolume;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupSlider();

        // Destroy self if entering a scene where the manager isn’t needed
        if (scene.name != "Main Title" && scene.name != "Levels Page")
        {
            if (TryGetComponent<AudioSource>(out var src))
                src.Stop();

            Destroy(gameObject);
        }
    }
}
