using UnityEngine;

// This script triggers a puzzle checkpoint in the Tutorial when the player enters the checkpoint zone.
public class CheckpointTrigger : MonoBehaviour
{
    // Puzzle Manager Instance
    public PuzzleManager puzzleManager;

    // The specific location associated with this checkpoint
    public Transform checkpointLocation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Notify the PuzzleManager to start the checkpoint logic
            puzzleManager.StartPuzzleCheckpoint(checkpointLocation);
        }
    }
}
