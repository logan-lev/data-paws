using UnityEngine;

// This script updates the player's respawn point when the player enters a designated checkpoint zone.
public class CheckpointZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Try to get the PlayerMovement component from the player object
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            // If the PlayerMovement script is found
            if (player != null)
            {
                // Update the player's respawn point to this zone's position
                player.UpdateRespawnPoint(transform.position);

                // If the player has a PuzzleManager assigned, update its checkpoint too
                if (player.puzzleManager != null)
                {
                    player.puzzleManager.UpdateCheckpoint(transform.position);
                }
            }
        }
    }
}