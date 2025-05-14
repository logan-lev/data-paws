using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int coinCount = 0;

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