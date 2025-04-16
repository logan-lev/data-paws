using UnityEngine;

public class BoxHole : MonoBehaviour
{
    public string correctBoxID;  // Match to boxID (e.g., "Box1")
    public bool isCorrect = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        BoxIdentifier id = other.GetComponent<BoxIdentifier>();
        if (id != null && id.boxID == correctBoxID)
        {
            isCorrect = true;
            Debug.Log($"{gameObject.name} matched boxID: {id.boxID}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        BoxIdentifier id = other.GetComponent<BoxIdentifier>();
        if (id != null && id.boxID == correctBoxID)
        {
            isCorrect = false;
            Debug.Log($"{gameObject.name} box exited: {id.boxID}");
        }
    }
}
