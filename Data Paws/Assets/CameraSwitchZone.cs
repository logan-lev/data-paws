using UnityEngine;

public class CameraSwitchZone : MonoBehaviour
{
    public Camera zoomedOutCamera;
    public Camera normalCamera;
    public TreeManager treeManager;

    private bool hasSwitched = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSwitched) return;

        if (other.CompareTag("Player"))
        {
            zoomedOutCamera.enabled = true;
            normalCamera.enabled = false;

            hasSwitched = true;

            // ✨ Puzzle starts when camera switches
            if (treeManager != null)
            {
                treeManager.ShowNextNumber();
            }

            // Optional: Destroy the trigger after switching
            Destroy(gameObject);
        }
    }
}
