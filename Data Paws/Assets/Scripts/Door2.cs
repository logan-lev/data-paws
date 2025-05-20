using UnityEngine;

// This script handles door behavior with two options:
// either destroy the door or simply deactivate it when opened.
public class Door2 : MonoBehaviour
{
    // Determines whether the door should be destroyed when opened
    [Header("Door Behavior")]
    public bool destroyOnOpen = false;

    // Call this method to open the door
    public void Open()
    {
        // If destroyOnOpen is true, completely remove the door from the scene
        if (destroyOnOpen)
        {
            Destroy(gameObject);
        }
        // Otherwise, just deactivate the GameObject (can be reactivated later if needed)
        else
        {
            gameObject.SetActive(false);
        }
    }
}