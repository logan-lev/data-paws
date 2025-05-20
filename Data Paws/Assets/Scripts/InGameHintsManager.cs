using UnityEngine;
using UnityEngine.SceneManagement;

// Manages the visibility of in-game hints UI across scenes using a persistent object
public class InGameHintsManager : MonoBehaviour
{
    // Reference to the background panel object used to show hints
    public GameObject bgPanel;

    // Static variable to remember the current visibility state of the hint panel
    private static bool isHintVisible = true;

    // Singleton instance to ensure only one copy persists across scenes
    private static InGameHintsManager instance;

    void Awake()
    {
        // Destroy duplicate instances if one already exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance and make this GameObject persistent across scenes
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // Attempt to find and link the hint UI in the current scene
        TryLinkBG();
    }

    void Update()
    {
        // Toggle the visibility of the hint panel when 'H' is pressed
        if (Input.GetKeyDown(KeyCode.H) && bgPanel != null)
        {
            isHintVisible = !isHintVisible;
            bgPanel.SetActive(isHintVisible);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-link the hint panel in the new scene
        TryLinkBG();
    }

    // Attempts to find the "BG" GameObject and link it as the hint panel
    private void TryLinkBG()
    {
        GameObject foundBG = GameObject.Find("BG");

        if (foundBG != null)
        {
            bgPanel = foundBG;
            bgPanel.SetActive(true);
        }
        else
        {
            bgPanel = null;
        }
    }
}