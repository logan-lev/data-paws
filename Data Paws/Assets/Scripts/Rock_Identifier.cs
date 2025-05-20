using UnityEngine;

// Identifies a rock with a specific ID and tracks whether it has been snapped into place
public class Rock_Identifier : MonoBehaviour
{
    // Unique identifier for this rock (used to match with Rock_Hole)
    public string rockID;

    // Flag to indicate if the rock has been placed/snapped in the correct hole
    public bool isSnapped = false;
}