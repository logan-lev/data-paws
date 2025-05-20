using UnityEngine;
using TMPro;

// Represents a single node in the tree puzzle where a number can be placed
public class TreeNode : MonoBehaviour
{
    // The correct value that should be placed in this node
    public int expectedValue = -1;

    // The current value placed by the player
    public int currentValue = -1;

    // Whether a value has been placed in this node
    public bool isFilled => currentValue != -1;

    // If true, this node is pre-filled and shouldn't be reset
    public bool alwaysFilled = false;

    // Reference to the text component displaying the node’s value
    public TMP_Text numberText;

    // Internal reference to the sprite renderer
    private SpriteRenderer sr;

    // Original color of the node, used to reset after flash
    private Color defaultColor;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
        UpdateNumberDisplay();
    }

    // Attempts to place a value in the node
    public void TryPlaceValue(int value, System.Action<bool> callback)
    {
        // Do nothing if node is already filled
        if (isFilled)
        {
            callback?.Invoke(false);
            return;
        }

        // If correct value, accept and flash blue
        if (value == expectedValue)
        {
            currentValue = value;
            UpdateNumberDisplay();
            FlashColor(Color.blue);
            callback?.Invoke(true);
        }
        else
        {
            // Wrong value, flash red
            FlashColor(Color.red);
            callback?.Invoke(false);
        }
    }

    // Resets the node’s value if it's not permanently filled
    public void ResetNode()
    {
        if (!alwaysFilled)
        {
            currentValue = -1;
            UpdateNumberDisplay();
        }
    }

    // Temporarily changes the node's color for feedback
    private void FlashColor(Color color)
    {
        sr.color = color;
        CancelInvoke(nameof(ResetColor));
        Invoke(nameof(ResetColor), 0.5f);
    }

    // Restores the node's original color
    private void ResetColor()
    {
        sr.color = defaultColor;
    }

    // Updates the text label to reflect the current value
    private void UpdateNumberDisplay()
    {
        if (numberText != null)
        {
            if (isFilled)
            {
                numberText.text = currentValue.ToString();
            }
            else
            {
                numberText.text = "";
            }
        }
    }
}