using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public PuzzleManager puzzleManager;
    public Transform checkpointLocation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            puzzleManager.StartPuzzleCheckpoint(checkpointLocation);
        }
    }
}
