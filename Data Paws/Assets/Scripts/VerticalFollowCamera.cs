using UnityEngine;

[RequireComponent(typeof(Camera))]
public class VerticalFollowCamera : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 3f;
    public float verticalMargin = 2f; // How far player can move up/down before camera starts following

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = currentPos;

        Vector2 delta = target.position - currentPos;

        if (delta.y > verticalMargin)
        {
            targetPos.y += delta.y - verticalMargin;
        }
        else if (delta.y < -verticalMargin)
        {
            targetPos.y += delta.y + verticalMargin;
        }

        transform.position = Vector3.Lerp(currentPos, targetPos, followSpeed * Time.deltaTime);
    }
}

