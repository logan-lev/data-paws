using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SideScrollingCamera : MonoBehaviour
{
    public Transform trackedObject;
    public float followSpeed = 5f;

    [Header("Dead Zone Margins")]
    public float horizontalMargin = 2f;  // How far the player can move left/right before camera moves
    public float verticalMargin = 2f;    // How far the player can move up/down before camera moves

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (trackedObject == null) return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = currentPos;

        // Get camera bounds in world units
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.aspect * halfHeight;

        // Get player's offset from camera center
        Vector2 delta = trackedObject.position - currentPos;

        // Horizontal dead zone
        if (delta.x > horizontalMargin)
        {
            targetPos.x += delta.x - horizontalMargin;
        }
        else if (delta.x < -horizontalMargin)
        {
            targetPos.x += delta.x + horizontalMargin;
        }

        // Vertical dead zone
        if (delta.y > verticalMargin)
        {
            targetPos.y += delta.y - verticalMargin;
        }
        else if (delta.y < -verticalMargin)
        {
            targetPos.y += delta.y + verticalMargin;
        }

        // Smooth follow
        transform.position = Vector3.Lerp(currentPos, targetPos, followSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || trackedObject == null || cam == null) return;

        Gizmos.color = Color.yellow;

        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.aspect * halfHeight;

        Vector3 center = transform.position;
        Vector3 size = new Vector3(horizontalMargin * 2, verticalMargin * 2, 0);

        Gizmos.DrawWireCube(center, size);
    }
}