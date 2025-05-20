using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static float globalVolume = 1f;
    private static bool volumeInitialized = false;

    public Slider musicSlider;
    public AudioSource audioSource;

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

        
        if (!volumeInitialized)
        {
            if (PlayerPrefs.HasKey(VolumeKey))
                globalVolume = PlayerPrefs.GetFloat(VolumeKey);
            else
                globalVolume = 1f; // default to max volume on launch

            volumeInitialized = true;
        }
    }

    void Start()
    {
        SetupSlider();
        SetMusicVolume();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void SetupSlider()
{
    // Find slider in scene if not assigned
    if (musicSlider == null)
    {
        musicSlider = GameObject.FindWithTag("MusicSlider")?.GetComponent<Slider>();
    }

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

    public void SetMusicVolume()
    {
        if (musicSlider != null)
            globalVolume = musicSlider.value;

        PlayerPrefs.SetFloat(VolumeKey, globalVolume);
        PlayerPrefs.Save();

        foreach (AudioSource src in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            if (src != null && src.enabled)
                src.volume = globalVolume;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    SetupSlider();

    if (scene.name != "Main Title" && scene.name != "Levels Page")
    {
        if (TryGetComponent<AudioSource>(out var src))
            src.Stop();

        Destroy(gameObject);
    }
}
}
