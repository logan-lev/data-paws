using UnityEngine;

// Handles logic for snapping a correct rock into a specific hole and notifying the puzzle manager
public class Rock_Hole : MonoBehaviour
{
    // The ID of the rock that belongs in this hole
    public string correctrockID;

    // Flag to track if the correct rock is placed
    public bool isCorrect = false;

    // Predefined snap positions for each rock
    private Vector3 snapPosition1 = new Vector2(117f, -23.3f);
    private Vector3 snapPosition2 = new Vector2(129f, -24.187f);
    private Vector3 snapPosition3 = new Vector2(125f, -24.4f);
    private Vector3 snapPosition4 = new Vector2(121.04f, -24.87f);

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object is a rock with a Rock_Identifier
        Rock_Identifier id = other.GetComponent<Rock_Identifier>();

        // Proceed only if the rock ID matches the correct ID for this hole
        if (id != null && id.rockID == correctrockID)
        {
            // Snap the rock to its assigned position
            if (id.rockID == "rock_1")
                other.transform.position = snapPosition1;
            else if (id.rockID == "rock_2")
                other.transform.position = snapPosition2;
            else if (id.rockID == "rock_3")
                other.transform.position = snapPosition3;
            else if (id.rockID == "rock_4")
                other.transform.position = snapPosition4;

            // Toggle the rock's collider to force trigger re-evaluation
            Collider2D rockCol = other.GetComponent<Collider2D>();
            if (rockCol != null)
            {
                rockCol.enabled = false;
                rockCol.enabled = true;
            }

            // Mark the rock as snapped
            id.isSnapped = true;

            // Change the rock’s layer to Ground to prevent further interactions
            other.gameObject.layer = LayerMask.NameToLayer("Ground");

            // Notify the PuzzleManager
            PuzzleManager manager = Object.FindFirstObjectByType<PuzzleManager>();
            if (manager != null)
            {
                // Re-enable collision between rock and player
                if (manager.playerCollider != null && rockCol != null)
                {
                    Physics2D.IgnoreCollision(rockCol, manager.playerCollider, false);
                }

                // Tell the manager that a correct placement was made
                manager.OnCorrectPlacement();
            }

            // Freeze the rock in place
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Static;
            }

            // Mark this hole as filled with the correct rock
            isCorrect = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If the correct rock exits, mark the hole as incorrect again
        Rock_Identifier id = other.GetComponent<Rock_Identifier>();
        if (id != null && id.rockID == correctrockID)
        {
            isCorrect = false;
        }
    }
}