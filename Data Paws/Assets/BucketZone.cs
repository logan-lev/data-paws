using UnityEngine;
using System.Collections;

public class BucketZone : MonoBehaviour
{
    public string bucketRange = "A-F"; // Set in Inspector
    public PuzzleManagerlvl2 puzzleManager;
    public SpriteRenderer feedbackZone; // The sprite that flashes color

    private void Start()
    {
        // Start invisible
        SetAlpha(0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PickupItem item = other.GetComponent<PickupItem>();

        // Only trigger if item is dropped (not being held)
        if (item != null && item.transform.parent == null)
        {
            char firstChar = char.ToLower(item.GetItemName()[0]);

            if (MatchesRange(firstChar))
            {
                FlashGreenAndDestroy(item.gameObject);
            }
            else
            {
                StartCoroutine(FlashRed());
            }
        }
    }

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

    private IEnumerator FlashRed()
    {
        SetAlpha(1f);
        feedbackZone.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        SetAlpha(0f);
    }

    public void FlashGreenAndDestroy(GameObject item)
    {
        StartCoroutine(FlashGreen(item));
    }

    private IEnumerator FlashGreen(GameObject item)
    {
        SetAlpha(1f);
        feedbackZone.color = Color.green;

        yield return new WaitForSeconds(0.5f);

        SetAlpha(0f);
        Destroy(item);
        puzzleManager.OnCorrectPlacement();
    }

    private void SetAlpha(float alpha)
    {
        Color c = feedbackZone.color;
        c.a = alpha;
        feedbackZone.color = c;
    }
}
