using UnityEngine;

public class Rock_Hole : MonoBehaviour
{
    public string correctrockID;  // Match to rockID (e.g., "Box1")
    public bool isCorrect = false;

    private Vector3 snapPosition1 = new Vector2(117f, -23.3f);
    private Vector3 snapPosition2 = new Vector2(129f, -24.187f);
    private Vector3 snapPosition3 = new Vector2(125f, -24.4f);
    private Vector3 snapPosition4 = new Vector2(121.04f, -24.87f);

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rock_Identifier id = other.GetComponent<Rock_Identifier>();
        if (id != null && id.rockID == correctrockID)
        {
            // Snap the rock to the correct position
            if (id.rockID == "rock_1")
                other.transform.position = snapPosition1;
            else if (id.rockID == "rock_2")
                other.transform.position = snapPosition2;
            else if (id.rockID == "rock_3")
                other.transform.position = snapPosition3;
            else if (id.rockID == "rock_4")
                other.transform.position = snapPosition4;

            // Force re-evaluate trigger by toggling collider
            Collider2D rockCol = other.GetComponent<Collider2D>();
            if (rockCol != null)
            {
                rockCol.enabled = false;
                rockCol.enabled = true;
            }

            id.isSnapped = true;
            other.gameObject.layer = LayerMask.NameToLayer("Ground");

            PuzzleManager manager = Object.FindFirstObjectByType<PuzzleManager>();
            if (manager != null)
            {
                // Re-enable collision with player
                if (manager.playerCollider != null && rockCol != null)
                {
                    Physics2D.IgnoreCollision(rockCol, manager.playerCollider, false);
                }

                // Notify puzzle manager
                manager.OnCorrectPlacement();
            }

            // Freeze the rock's physics
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Static;
            }

            isCorrect = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Rock_Identifier id = other.GetComponent<Rock_Identifier>();
        if (id != null && id.rockID == correctrockID)
        {
            isCorrect = false;
        }
    }
}
