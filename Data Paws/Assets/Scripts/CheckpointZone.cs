using UnityEngine;

public class CheckpointZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
if (player != null)
{
    player.UpdateRespawnPoint(transform.position);
    
    if (player.puzzleManager != null)
    {
        player.puzzleManager.UpdateCheckpoint(transform.position);
    }
}
        }
    }
}
