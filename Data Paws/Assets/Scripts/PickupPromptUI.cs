using UnityEngine;
using TMPro;

// This script displays a UI prompt when the player is near a pickup item.
public class PickupPromptUI : MonoBehaviour
{
    // Reference to the TMP text element used to show the pickup prompt
    public TMP_Text pickupPromptText;

    // The range within which the prompt will appear
    public float pickupRange = 1.5f;

    // Layer mask to detect pickup items
    public LayerMask itemLayer;

    // Tracks whether the prompt is currently shown
    private bool promptShown = false;

    private void Start()
    {
        // Hide the prompt at the start
        if (pickupPromptText != null)
            pickupPromptText.gameObject.SetActive(false);

        promptShown = false;
    }

    private void Update()
    {
        // Tracks whether the prompt should be visible this frame
        bool shouldShow = false;

        // Check for nearby colliders within the pickup range and item layer
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange, itemLayer);

        // Loop through all found colliders
        foreach (Collider2D hit in hits)
        {
            // Get the PickupItem component
            PickupItem item = hit.GetComponent<PickupItem>();

            // Only show the prompt if the item is not currently held
            if (item != null && item.transform.parent == null)
            {
                shouldShow = true;
                break;
            }
        }

        // Show the prompt if needed and it's not already shown
        if (shouldShow && !promptShown)
        {
            ShowPrompt();
        }
        // Hide the prompt if it's shown but no valid items are nearby
        else if (!shouldShow && promptShown)
        {
            HidePrompt();
        }
    }

    // Activates the pickup prompt UI
    private void ShowPrompt()
    {
        if (pickupPromptText != null)
            pickupPromptText.gameObject.SetActive(true);

        promptShown = true;
    }

    // Deactivates the pickup prompt UI
    private void HidePrompt()
    {
        if (pickupPromptText != null)
            pickupPromptText.gameObject.SetActive(false);

        promptShown = false;
    }

    // Draws a visual representation of the pickup range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}