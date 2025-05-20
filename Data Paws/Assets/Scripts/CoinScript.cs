using UnityEngine;

// This script handles individual coin behavior.
public class CoinScript : MonoBehaviour
{
    // Unique identifier for this coin (set in Inspector)
    // Used to track whether it has been collected before
    [Tooltip("Must be unique per coin, e.g., tutorial-coin1")]
    public string coinID;

    void Start()
    {
        // Check if this coin was already collected in a previous session
        if (PlayerPrefs.GetInt("CoinCollected_" + coinID, 0) == 1)
        {
            // If so, destroy it immediately so it doesn't appear
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Save that this specific coin has been collected
            PlayerPrefs.SetInt("CoinCollected_" + coinID, 1);
            PlayerPrefs.Save();

            // Add this coin to the player's total via the CoinManager
            CoinManager.instance.AddCoin();

            // Remove the coin from the scene
            Destroy(gameObject);
        }
    }
}