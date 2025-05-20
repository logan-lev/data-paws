using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameHintsManager : MonoBehaviour
{
    public GameObject bgPanel;

    private static bool isHintVisible = true;
    private static InGameHintsManager instance;

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
        TryLinkBG();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && bgPanel != null)
        {
            isHintVisible = !isHintVisible;
            bgPanel.SetActive(isHintVisible);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryLinkBG();
    }

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