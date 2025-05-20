using UnityEngine;

// Requires a Camera component to be attached to the GameObject
[RequireComponent(typeof(Camera))]
public class SideScrCamLvl2 : MonoBehaviour
{
    // The target the camera should follow (e.g., player)
    public Transform trackedObject;

    // How quickly the camera should follow the target
    public float followSpeed = 5f;

    // Whether the camera should follow the target
    public bool cameraFollowEnabled = true;

    [Header("Dead Zone Margins")]
    // Horizontal and vertical margins to define the camera's dead zone
    public float horizontalMargin = 2f;
    public float verticalMargin = 2f;

    [Header("Camera Bounds")]
    // Whether to clamp camera position within specified bounds
    public bool useCameraLimits = true;

    // Minimum and maximum x and y positions for the camera
    public float minX = -10f;
    public float maxX = 50f;
    public float minY = -5f;
    public float maxY = 10f;

    // Reference to the Camera component
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // Called after all Update functions — ideal for camera movement
    private void LateUpdate()
    {
        // If following is disabled or no target is set, skip
        if (!cameraFollowEnabled || trackedObject == null) return;

        // Store current camera position
        Vector3 currentPos = transform.position;

        // Target position to interpolate toward
        Vector3 targetPos = currentPos;

        // Calculate camera's visible size in world units
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.aspect * halfHeight;

        // Difference between tracked object and current camera position
        Vector2 delta = trackedObject.position - currentPos;

        // Adjust horizontal camera position if outside margin
        if (delta.x > horizontalMargin)
        {
            targetPos.x += delta.x - horizontalMargin;
        }
        else if (delta.x < -horizontalMargin)
        {
            targetPos.x += delta.x + horizontalMargin;
        }

        // Adjust vertical camera position if outside margin
        if (delta.y > verticalMargin)
        {
            targetPos.y += delta.y - verticalMargin;
        }
        else if (delta.y < -verticalMargin)
        {
            targetPos.y += delta.y + verticalMargin;
        }

        // Clamp camera position to specified bounds
        if (useCameraLimits)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, minX + halfWidth, maxX - halfWidth);
            targetPos.y = Mathf.Clamp(targetPos.y, minY + halfHeight, maxY - halfHeight);
        }

        // Smoothly interpolate the camera to the target position
        transform.position = Vector3.Lerp(currentPos, targetPos, followSpeed * Time.deltaTime);
    }

    // Draws a visual representation of the dead zone in the Scene view
    private void OnDrawGizmosSelected()
    {
        // Skip drawing if not in play mode or required references are missing
        if (!Application.isPlaying || trackedObject == null || cam == null) return;

        // Set Gizmo color
        Gizmos.color = Color.yellow;

        // Calculate camera dimensions
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.aspect * halfHeight;

        // Draw the dead zone rectangle centered on the camera
        Vector3 center = transform.position;
        Vector3 size = new Vector3(horizontalMargin * 2, verticalMargin * 2, 0);
        Gizmos.DrawWireCube(center, size);
    }
}