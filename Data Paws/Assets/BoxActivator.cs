using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoxActivator : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool activated = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        FreezeBox();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!activated && collision.collider.CompareTag("Player"))
        {
            ActivateBox();
        }
    }

    private void FreezeBox()
    {
        rb.bodyType = RigidbodyType2D.Static; // Frozen
    }

    private void ActivateBox()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // Enable movement
        activated = true;
    }
}
