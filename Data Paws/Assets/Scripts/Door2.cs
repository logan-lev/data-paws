using UnityEngine;

public class Door2 : MonoBehaviour
{
    [Header("Door Behavior")]
    public bool destroyOnOpen = false;

    public void Open()
    {

        if (destroyOnOpen)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

