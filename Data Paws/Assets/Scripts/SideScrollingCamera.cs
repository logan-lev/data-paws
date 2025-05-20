using UnityEngine;

// Ensures that the GameObject this script is attached to has a Camera component
[RequireComponent(typeof(Camera))]
public class SideScrollingCamera : MonoBehaviour
{
    // The object the camera should follow (usually the player)
    public Transform trackedObject;

    // How quickly the camera follows the target
    public float followSpeed = 5f;

    // Flag to enable or disable camera following behavior
    public bool cameraFollowEnabled = true;

    [Header("Dead Zone Margins")]
    // Horizontal margin before camera starts following horizontally
    public float horizontalMargin = 2f;

    // Vertical margin before camera starts following vertically
    public float verticalMargin = 2f;

    [Header("Camera Bounds")]
    // Whether to constrain the camera within defined boundaries
    public bool useCameraLimits = true;

    // Minimum X boundary for camera position
    public float minX = -10f;

    // Maximum X boundary for camera position
    public float maxX = 50f;

    // Minimum Y boundary for camera position
    public float minY = -5f;

    // Maximum Y boundary for camera position
    public float maxY = 10f;

    // Reference to the camera component
    private Camera cam;
    
    private void Awake()
    {
        // Cache the Camera component
        cam = GetComponent<Camera>();
    }

    // Called after all Update methods, ideal for camera movement
    private void LateUpdate()
    {
        // Do nothing if camera following is disabled or no target is set
        if (!cameraFollowEnabled || trackedObject == null) return;

        // Current camera position
        Vector3 currentPos = transform.position;

        // Target position the camera should move toward
        Vector3 targetPos = currentPos;

        // Get half the camera view size
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.aspect * halfHeight;

        // Get the difference between the tracked object and camera position
        Vector2 delta = trackedObject.position - currentPos;

        // Check if object moved beyond horizontal margin
        if (delta.x > horizontalMargin)
        {
            targetPos.x += delta.x - horizontalMargin;
        }
        else if (delta.x < -horizontalMargin)
        {
            targetPos.x += delta.x + horizontalMargin;
        }

        // Check if object moved beyond vertical margin
        if (delta.y > verticalMargin)
        {
            targetPos.y += delta.y - verticalMargin;
        }
        else if (delta.y < -verticalMargin)
        {
            targetPos.y += delta.y + verticalMargin;
        }

        // Clamp camera position within defined bounds if enabled
        if (useCameraLimits)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, minX + halfWidth, maxX - halfWidth);
            targetPos.y = Mathf.Clamp(targetPos.y, minY + halfHeight, maxY - halfHeight);
        }

        // Smoothly interpolate from current position to target position
        transform.position = Vector3.Lerp(currentPos, targetPos, followSpeed * Time.deltaTime);
    }

    // Draws a visual representation of the camera's dead zone in the editor
    private void OnDrawGizmosSelected()
    {
        // Only draw when the application is running and references are valid
        if (!Application.isPlaying || trackedObject == null || cam == null) return;

        // Set gizmo color to yellow
        Gizmos.color = Color.yellow;

        // Get camera bounds
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.aspect * halfHeight;

        // Center and size of the dead zone
        Vector3 center = transform.position;
        Vector3 size = new Vector3(horizontalMargin * 2, verticalMargin * 2, 0);

        // Draw the dead zone as a wireframe box
        Gizmos.DrawWireCube(center, size);
    }
}