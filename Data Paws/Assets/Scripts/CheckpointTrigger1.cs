using UnityEngine;

public class CheckpointTrigger1 : MonoBehaviour
{
    public TreeManager treeManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && treeManager != null)
        {
            treeManager.UpdateCheckpoint(transform.position);
        }
    }
}

