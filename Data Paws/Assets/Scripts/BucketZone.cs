using UnityEngine;
using System.Collections;

// This script defines behavior for a "bucket" zone where items are sorted based on their name in Level 2.
public class BucketZone : MonoBehaviour
{
    // Set in Inspector; Defines the valid letter range for this bucket
    public string bucketRange = "A-F";

    // Puzzle Manager Instance
    public PuzzleManagerlvl2 puzzleManager;

    // Colored Sprite for the Feedback Zone
    public SpriteRenderer feedbackZone;

    private void Start()
    {
        // Make the feedback zone initially invisible
        SetAlpha(0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the incoming object is a PickupItem
        PickupItem item = other.GetComponent<PickupItem>();

        // Only trigger if item is dropped (not being held)
        if (item != null && item.transform.parent == null)
        {
            // Check the first character of the item's name
            char firstChar = char.ToLower(item.GetItemName()[0]);

            // If the letter matches this bucket's assigned range
            if (MatchesRange(firstChar))
            {
                // Flash green and destroy the item
                FlashGreenAndDestroy(item.gameObject);
            }
            else
            {
                // Flash red (incorrect item)
                StartCoroutine(FlashRed());
            }
        }
    }

    // Determines whether the item's starting letter matches this bucket's range
    private bool MatchesRange(char c)
    {
        switch (bucketRange)
        {
            case "A-F": return c >= 'a' && c <= 'f';
            case "G-M": return c >= 'g' && c <= 'm';
            case "N-T": return c >= 'n' && c <= 't';
            case "U-Z": return c >= 'u' && c <= 'z';
        }
        return false;
    }

    // Coroutine to flash red briefly to show incorrect placement
    private IEnumerator FlashRed()
    {
        SetAlpha(1f);
        feedbackZone.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        SetAlpha(0f);
    }

    // Starts the green flash coroutine and destroys the correct item
    public void FlashGreenAndDestroy(GameObject item)
    {
        StartCoroutine(FlashGreen(item));
    }

    // Coroutine to flash green and destroy the object
    private IEnumerator FlashGreen(GameObject item)
    {
        SetAlpha(1f);
        feedbackZone.color = Color.green;

        yield return new WaitForSeconds(0.5f);

        SetAlpha(0f);
        Destroy(item);
        puzzleManager.OnCorrectPlacement();
    }

    // Utility function to set the alpha (transparency) of the feedback sprite
    private void SetAlpha(float alpha)
    {
        Color c = feedbackZone.color;
        c.a = alpha;
        feedbackZone.color = c;
    }
}
