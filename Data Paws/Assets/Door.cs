using UnityEngine;

public class Door : MonoBehaviour
{
    public void Open()
    {
        // Disable collider
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Disable sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;
    }
}