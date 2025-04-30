using UnityEngine;
using TMPro;

public class PickupPromptUI : MonoBehaviour
{
    public TMP_Text pickupPromptText;
    public float pickupRange = 1.5f;
    public LayerMask itemLayer;

    private bool promptShown = false;

    private void Update()
    {
        bool shouldShow = false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange, itemLayer);
        foreach (Collider2D hit in hits)
        {
            PickupItem item = hit.GetComponent<PickupItem>();
            if (item != null && item.transform.parent == null) // only if not being held
            {
                shouldShow = true;
                break;
            }
        }

        if (shouldShow && !promptShown)
        {
            ShowPrompt();
        }
        else if (!shouldShow && promptShown)
        {
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        if (pickupPromptText != null)
            pickupPromptText.gameObject.SetActive(true);

        promptShown = true;
    }

    private void HidePrompt()
    {
        if (pickupPromptText != null)
            pickupPromptText.gameObject.SetActive(false);

        promptShown = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
