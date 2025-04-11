using UnityEngine;

public class BoxHole : MonoBehaviour
{
    public string correctBoxTag;  // Assign in Inspector (e.g., "Box1")
    public bool isCorrect = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(correctBoxTag))
            isCorrect = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(correctBoxTag))
            isCorrect = false;
    }
}
