using UnityEngine;

// Triggers the start of a puzzle when the player enters a trigger zone
public class PuzzleTrigger : MonoBehaviour
{
    // Reference to the Level 2 Puzzle Manager
    public PuzzleManagerlvl2 puzzleManagerlvl2; // ✅ Updated type

    // Camera to switch to when the puzzle starts
    public Camera zoomedOutCamera;

    // Player's normal gameplay camera
    public Camera playerCamera;

    // Flag to prevent triggering more than once
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Do nothing if already triggered
        if (triggered) return;

        // Only trigger when the player enters
        if (other.CompareTag("Player"))
        {
            // Mark as triggered so it doesn’t fire again
            triggered = true;

            // Start the puzzle using the puzzle manager
            if (puzzleManagerlvl2 != null)
                puzzleManagerlvl2.StartPuzzle();

            // Switch camera views if references are set
            if (zoomedOutCamera != null) zoomedOutCamera.enabled = true;
            if (playerCamera != null) playerCamera.enabled = false;
        }
    }
}