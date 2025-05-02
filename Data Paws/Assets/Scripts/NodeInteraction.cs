using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeInteraction : MonoBehaviour
{
    public float interactRange = 1.5f;
    public LayerMask nodeLayer;
    public TMP_Text interactText; // Assign your Interact UI Text here
    public TreeManager treeManager; // Assign in Inspector

    private TreeNode closestNode;

    private void Update()
    {
        FindClosestNode();

        if (closestNode != null)
        {
            interactText.gameObject.SetActive(true);
            interactText.text = "Press E to interact";

            if (Input.GetKeyDown(KeyCode.E))
            {
                closestNode.TryPlaceValue(treeManager.GetCurrentNumber(), success => {
                    if (success)
                    {
                        treeManager.NextNumber();
                    }
                });
            }
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }

    private void FindClosestNode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, nodeLayer);

        float closestDistance = Mathf.Infinity;
        TreeNode foundNode = null;

        foreach (var hit in hits)
        {
            TreeNode node = hit.GetComponent<TreeNode>();
            if (node != null && !node.isFilled)
            {
                float dist = Vector2.Distance(transform.position, node.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    foundNode = node;
                }
            }
        }

        closestNode = foundNode;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}

