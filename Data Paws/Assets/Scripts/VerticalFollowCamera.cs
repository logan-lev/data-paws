using UnityEngine;

// Ensures the GameObject has a Camera component
[RequireComponent(typeof(Camera))]
public class VerticalFollowCamera : MonoBehaviour
{
    // The target object the camera should follow vertically (e.g., the player)
    public Transform target;

    // How fast the camera follows the target
    public float followSpeed = 3f;

    // Margin of vertical movement before the camera starts to follow
    public float verticalMargin = 2f;

    // Internal reference to the Camera component
    private Camera cam;

    // Called when the script is initialized
    private void Awake()
    {
        // Cache the Camera component
        cam = GetComponent<Camera>();
    }

    // Called after all Update methods — good for camera movement
    private void LateUpdate()
    {
        // Exit if no target is assigned
        if (target == null) return;

        // Store current and target camera positions
        Vector3 currentPos = transform.position;
        Vector3 targetPos = currentPos;

        // Calculate vertical difference between target and camera
        Vector2 delta = target.position - currentPos;

        // Adjust the camera's Y position if the target moves beyond the vertical margin
        if (delta.y > verticalMargin)
        {
            targetPos.y += delta.y - verticalMargin;
        }
        else if (delta.y < -verticalMargin)
        {
            targetPos.y += delta.y + verticalMargin;
        }

        // Smoothly interpolate from current position to target position
        transform.position = Vector3.Lerp(currentPos, targetPos, followSpeed * Time.deltaTime);
    }
}