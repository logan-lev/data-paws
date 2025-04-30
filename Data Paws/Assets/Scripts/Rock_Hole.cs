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
            // Snap the rock to the hole
            if (id.rockID == "rock_1")
            {
                other.transform.position = snapPosition1;
            }
            if (id.rockID == "rock_2")
            {
                other.transform.position = snapPosition2;
            }
            if (id.rockID == "rock_3")
            {
                other.transform.position = snapPosition3;
            }
            if (id.rockID == "rock_4")
            {
                other.transform.position = snapPosition4;
            }

            id.isSnapped = true;

            // Stop physics motion
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Static; // Freeze it
            }

            // Mark this hole as solved
            isCorrect = true;

            Debug.Log($"{gameObject.name} matched and snapped rockID: {id.rockID}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Rock_Identifier id = other.GetComponent<Rock_Identifier>();
        if (id != null && id.rockID == correctrockID)
        {
            isCorrect = false;
            Debug.Log($"{gameObject.name} box exited: {id.rockID}");
        }
    }
}
