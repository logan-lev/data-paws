using UnityEngine;
using TMPro;

public class PlayerPickup : MonoBehaviour
{
    public Transform holdPoint;
    public float pickupRange = 3f;
    public LayerMask itemLayer;
    public TMP_Text pickupPromptText;

    private Rigidbody2D rb;
    private PickupItem heldItem;
    private float lastInputDirection = 1f; // 1 = right, -1 = left

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Track last direction input
        float inputX = Input.GetAxisRaw("Horizontal");
        if (inputX != 0)
        {
            lastInputDirection = Mathf.Sign(inputX);
        }

        // Show/hide pickup prompt based on range
        UpdatePickupPrompt();

        // Handle pickup/drop input
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
            {
                TryPickup();
            }
            else
            {
                DropItem();
            }
        }
    }

    private void TryPickup()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange, itemLayer);
        foreach (Collider2D hit in hits)
        {
            PickupItem item = hit.GetComponent<PickupItem>();
            if (item != null && item.transform.parent == null)
            {
                heldItem = item;
                item.PickUp(holdPoint);
                return;
            }
        }
    }

    private void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);

            // Drop farther in front and slightly higher
            Vector2 dropOffset = new Vector2(lastInputDirection * 1.5f, 1f);
            heldItem.transform.position = (Vector2)transform.position + dropOffset;

            heldItem.GetComponent<Collider2D>().enabled = true;
            heldItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            heldItem = null;
        }
    }

    private void UpdatePickupPrompt()
    {
        bool showPrompt = false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange, itemLayer);
        foreach (Collider2D hit in hits)
        {
            PickupItem item = hit.GetComponent<PickupItem>();
            if (item != null && item.transform.parent == null)
            {
                showPrompt = true;
                break;
            }
        }

        if (pickupPromptText != null)
            pickupPromptText.enabled = showPrompt;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
