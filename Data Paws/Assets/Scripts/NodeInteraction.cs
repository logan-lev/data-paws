using UnityEngine;
using TMPro;

// This script allows the player to interact with nearby tree nodes.
public class NodeInteraction : MonoBehaviour
{
    // Range within which the player can interact with nodes
    public float interactRange = 1.5f;

    // Layer mask to filter which objects are considered tree nodes
    public LayerMask nodeLayer;

    // Reference to the UI text shown when interaction is possible
    public TMP_Text interactText;

    // Reference to the TreeManager controlling puzzle logic
    public TreeManager treeManager;

    // The closest valid tree node for interaction
    private TreeNode closestNode;

    private void Update()
    {
        // Look for the nearest valid node within interaction range
        FindClosestNode();

        // If a valid node is found
        if (closestNode != null)
        {
            // Show the interact UI text
            interactText.gameObject.SetActive(true);
            interactText.text = "Press E to interact";

            // If the player presses 'E'
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Attempt to place the current value in the node
                closestNode.TryPlaceValue(treeManager.GetCurrentNumber(), success => {
                    // If successful, move to the next number in the sequence
                    if (success)
                    {
                        treeManager.NextNumber();
                    }
                });
            }
        }
        // If no valid node is nearby, hide the interaction UI
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }

    // Searches for the closest unfilled TreeNode within range
    private void FindClosestNode()
    {
        // Get all colliders within the interaction range on the node layer
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, nodeLayer);

        // Initialize tracking for the nearest node
        float closestDistance = Mathf.Infinity;
        TreeNode foundNode = null;

        // Iterate over all detected colliders
        foreach (var hit in hits)
        {
            TreeNode node = hit.GetComponent<TreeNode>();

            // Check if it's a valid unfilled node
            if (node != null && !node.isFilled)
            {
                // Calculate distance from the player
                float dist = Vector2.Distance(transform.position, node.transform.position);

                // Update the closest node if this one is nearer
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    foundNode = node;
                }
            }
        }

        // Set the closest node found this frame
        closestNode = foundNode;
    }

    // Draws a visual representation of the interaction radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}