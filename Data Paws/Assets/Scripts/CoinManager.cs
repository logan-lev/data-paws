using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script manages the player's coin count and displays it across scenes.
public class CoinManager : MonoBehaviour
{
    // Coin Manager Instance
    public static CoinManager instance;

    // Total number of coins collected
    public int coinCount = 0;

    // Coin Text UI
    public TMP_Text coinText;

    void Awake()
    {
        // Enforce singleton pattern
        if (instance == null)
        {
            instance = this;

            // Detach from parent just in case it's nested in a prefab
            transform.SetParent(null);

            // Persist this object across scene loads
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy duplicate instances
            Destroy(gameObject);
            return;
        }

        // If the coinText reference exists, initialize it
        if (coinText != null)
        {
            coinText.text = coinCount.ToString() + " / 7 Coins Collected";
        }
    }

    void Start()
    {
        // Ensure the coin UI is set up for the starting scene
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to find the TMP_Text tagged as "CoinText" in the new scene
        GameObject found = GameObject.FindWithTag("CoinText");
        
        if (found != null)
        {
            coinText = found.GetComponent<TMP_Text>();
        }
        else
        {
            coinText = null;
        }

        // Determine whether to show the coin UI in this scene
        bool showCoinUI = scene.name == "Tutorial" || scene.name == "Level 1" || scene.name == "Level 2" || scene.name == "End Screen";

        // Show or hide the coin text UI based on scene
        if (coinText != null)
        {
            coinText.gameObject.SetActive(showCoinUI);

            // Update the displayed coin count if UI is shown
            if (showCoinUI)
                coinText.text = coinCount.ToString() + " / 7 Coins Collected";
        }

        // Reset coin count when returning to the main title
        if (scene.name == "Main Title")
        {
            coinCount = 0;
            Debug.Log("Coins: " + coinCount);
        }
    }

    // Increases the coin count by 1 and updates the UI
    public void AddCoin()
    {
        coinCount++;
        Debug.Log("Coins: " + coinCount);

        // Update coin text only if it exists and is currently active
        if (coinText != null && coinText.gameObject.activeSelf)
        {
            coinText.text = coinCount.ToString() + " / 7 Coins Collected";
        }
    }

    // Returns the current total number of coins collected
    public int GetTotalCoins()
    {
        return coinCount;
    }
}
