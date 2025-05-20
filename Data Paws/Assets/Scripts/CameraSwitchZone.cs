using UnityEngine;

// This script handles switching the camera when the player enters a trigger zone.
public class CameraSwitchZone : MonoBehaviour
{
    // Camera to activate when the player enters the zone
    public Camera zoomedOutCamera;

    // Camera to deactivate when the player enters the zone
    public Camera normalCamera;

    // Tree Manager Instance
    public TreeManager treeManager;

    // Flag to ensure the switch only happens once
    private bool hasSwitched = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If we've already switched once, do nothing
        if (hasSwitched) return;

        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Enable the zoomed-out camera
            zoomedOutCamera.enabled = true;

            // Disable the normal camera
            normalCamera.enabled = false;

            // Prevent further switching
            hasSwitched = true;

            // If a TreeManager is assigned, start the puzzle
            if (treeManager != null)
            {
                treeManager.ShowNextNumber();
            }

            Destroy(gameObject);
        }
    }
}