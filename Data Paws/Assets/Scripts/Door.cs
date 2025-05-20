using UnityEngine;

// This script represents a door that can be "opened" by disabling its collider and visual appearance.
public class Door : MonoBehaviour
{
    // Call this method to open the door
    public void Open()
    {
        // Get the Collider2D component attached to this GameObject
        Collider2D col = GetComponent<Collider2D>();

        // If a collider is found, disable it to allow the player to pass through
        if (col != null)
            col.enabled = false;

        // Get the SpriteRenderer component attached to this GameObject
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // If a sprite renderer is found, disable it to hide the door visually
        if (sr != null)
            sr.enabled = false;
    }
}