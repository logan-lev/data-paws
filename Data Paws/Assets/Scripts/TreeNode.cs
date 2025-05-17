using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreeNode : MonoBehaviour
{
    public int expectedValue = -1; // Correct number
    public int currentValue = -1;  // Placed number
    public bool isFilled => currentValue != -1;
    public bool alwaysFilled = false;


    public TMP_Text numberText; // Assign a Text component for showing the number

    private SpriteRenderer sr;
    private Color defaultColor;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
        UpdateNumberDisplay();
    }

    public void TryPlaceValue(int value, System.Action<bool> callback)
    {
        if (isFilled)
        {
            callback?.Invoke(false);
            return;
        }

        if (value == expectedValue)
        {
            currentValue = value;
            UpdateNumberDisplay();
            FlashColor(Color.blue);
            callback?.Invoke(true);
        }
        else
        {
            FlashColor(Color.red);
            callback?.Invoke(false);
        }
    }
    public void ResetNode()
{
    if (!alwaysFilled)
    {
        currentValue = -1;
        UpdateNumberDisplay();
    }
}


    private void FlashColor(Color color)
    {
        sr.color = color;
        CancelInvoke(nameof(ResetColor));
        Invoke(nameof(ResetColor), 0.5f);
    }

    private void ResetColor()
    {
        sr.color = defaultColor;
    }

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
