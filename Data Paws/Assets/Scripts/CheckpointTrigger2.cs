using UnityEngine;

// This script sets a checkpoint in Level 2 when the player enters the trigger.
public class CheckpointTrigger2 : MonoBehaviour
{
    // Puzzle Manager Instance
    public PuzzleManagerlvl2 puzzleManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player and if the puzzleManager is assigned
        if (other.CompareTag("Player") && puzzleManager != null)
        {
            // Try to find a child object named "SpawnPoint"
            Transform spawn = transform.Find("SpawnPoint");

            // Use SpawnPoint's position if found, otherwise use this object's position
            Vector3 pos = spawn != null ? spawn.position : transform.position;

            // Update the checkpoint position in the PuzzleManager
            puzzleManager.UpdateCheckpoint(pos);
        }
    }
}
