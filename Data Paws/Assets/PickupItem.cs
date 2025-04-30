using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName;
    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void PickUp(Transform holdPoint)
    {
        col.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
    }

    public void Drop(Vector2 throwDirection)
    {
        transform.SetParent(null);
        rb.bodyType = RigidbodyType2D.Dynamic;
        col.enabled = true;

        // Add a gentle throw in the given direction
        rb.linearVelocity = throwDirection * 3f;
    }

    public string GetItemName()
    {
        return itemName;
    }
}
