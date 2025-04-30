using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    public PuzzleManagerlvl2 puzzleManagerlvl2; // ✅ Updated type
    public Camera zoomedOutCamera;
    public Camera playerCamera;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            // Start the puzzle logic
            if (puzzleManagerlvl2 != null)
                puzzleManagerlvl2.StartPuzzle();

            // Optional: switch to zoomed out camera
            if (zoomedOutCamera != null) zoomedOutCamera.enabled = true;
            if (playerCamera != null) playerCamera.enabled = false;
        }
    }
}
