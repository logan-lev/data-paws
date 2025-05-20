using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [Tooltip("Must be unique per coin, e.g., tutorial-coin1")]
    public string coinID;

    void Start()
    {
        if (PlayerPrefs.GetInt("CoinCollected_" + coinID, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Mark this coin as collected
            PlayerPrefs.SetInt("CoinCollected_" + coinID, 1);
            PlayerPrefs.Save();

            CoinManager.instance.AddCoin();
            Destroy(gameObject);
        }
    }
}
