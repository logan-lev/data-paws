using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int coinCount = 0;
    public TMP_Text coinText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null); // make sure it's a root object
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Show initial text only if UI is present
        if (coinText != null)
        {
            coinText.text = coinCount.ToString() + " / 7 Coins Collected";
        }
    }

    void Start()
    {
        // Call scene visibility check on first load
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
        // Try to find the CoinText in the new scene
        GameObject found = GameObject.FindWithTag("CoinText");
        if (found != null)
        {
            coinText = found.GetComponent<TMP_Text>();
        }
        else
        {
            coinText = null;
        }

        // Show or hide based on scene name
        bool showCoinUI = scene.name == "Tutorial" || scene.name == "Level 1" || scene.name == "Level 2" || scene.name == "End Screen";

        if (coinText != null)
        {
            coinText.gameObject.SetActive(showCoinUI);
            if (showCoinUI)
                coinText.text = coinCount.ToString() + " / 7 Coins Collected";
        }

        if (scene.name == "Main Title")
        {
            coinCount = 0;
            Debug.Log("Coins: " + coinCount);
        }
}

    public void AddCoin()
    {
        coinCount++;
        Debug.Log("Coins: " + coinCount);

        if (coinText != null && coinText.gameObject.activeSelf)
        {
            coinText.text = coinCount.ToString() + " / 7 Coins Collected";
        }
    }

    public int GetTotalCoins()
    {
        return coinCount;
    }
}
