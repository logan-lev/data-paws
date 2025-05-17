using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int coinCount = 0;
    public Text coinText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ✅ Keep across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        if (instance != null)
        {
            int total = instance.coinCount;
            coinText.text = total.ToString() + " / 7 Coins Collected";
        }

    }

    public void AddCoin()
    {
        coinCount++;
        Debug.Log("Coins: " + coinCount);
    }

    public int GetTotalCoins()
    {
        return coinCount;
    }
}