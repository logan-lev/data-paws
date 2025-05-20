using UnityEngine;

// This script allows an item to be picked up and dropped by the player.
public class PickupItem : MonoBehaviour
{
    // The name of the item, used for display or logic
    public string itemName;

    // Cached reference to the item's Rigidbody2D component
    private Rigidbody2D rb;

    // Cached reference to the item's Collider2D component
    private Collider2D col;

    private void Awake()
    {
        // Get and store references to Rigidbody2D and Collider2D
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Called when the item is picked up
    public void PickUp(Transform holdPoint)
    {
        // Disable the collider to prevent unwanted collisions
        col.enabled = false;

        // Set the body type to kinematic so it's not affected by physics
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Stop any current movement
        rb.linearVelocity = Vector2.zero;

        // Attach the item to the hold point (e.g. player's hand)
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
    }

    // Called when the item is dropped
    public void Drop(Vector2 throwDirection)
    {
        // Detach the item from its parent
        transform.SetParent(null);

        // Set the body type back to dynamic to re-enable physics
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Re-enable the collider for interaction
        col.enabled = true;

        // Apply a light throw in the specified direction
        rb.linearVelocity = throwDirection * 3f;
    }

    // Returns the name of the item
    public string GetItemName()
    {
        return itemName;
    }
}