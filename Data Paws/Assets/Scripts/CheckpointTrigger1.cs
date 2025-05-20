using UnityEngine;

// This script updates the player's checkpoint in Level 1 when they enter a trigger zone.
public class CheckpointTrigger1 : MonoBehaviour
{
    // Tree Manager Instance
    public TreeManager treeManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player and if TreeManager is assigned
        if (other.CompareTag("Player") && treeManager != null)
        {
            // Update the checkpoint position in the TreeManager using this object's position
            treeManager.UpdateCheckpoint(transform.position);
        }
    }
}