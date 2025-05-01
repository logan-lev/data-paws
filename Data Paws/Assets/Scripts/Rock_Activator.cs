using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rock_Activator : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool activated = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        FreezeRock();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!activated && collision.collider.CompareTag("Player"))
        {
            ActivateRock();
        }
    }

    private void FreezeRock()
    {
        rb.bodyType = RigidbodyType2D.Static; // Frozen
    }

    private void ActivateRock()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // Enable movement
        activated = true;
    }
}
