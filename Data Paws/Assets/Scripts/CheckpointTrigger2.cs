using UnityEngine;

public class CheckpointTrigger2 : MonoBehaviour
{
    public PuzzleManagerlvl2 puzzleManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && puzzleManager != null)
        {
            Transform spawn = transform.Find("SpawnPoint");
            Vector3 pos = spawn != null ? spawn.position : transform.position;
            puzzleManager.UpdateCheckpoint(pos);
        }
    }
}

