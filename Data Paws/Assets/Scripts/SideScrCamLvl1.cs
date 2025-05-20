using UnityEngine;

// Enforces that this script is attached to a GameObject with a Camera
[RequireComponent(typeof(Camera))]
public class SideScrCamLvl1 : MonoBehaviour
{
    // The object the camera should follow (e.g., the player)
    public Transform trackedObject;

    // Speed at which the camera follows the tracked object
    public float followSpeed = 5f;

    // Whether the camera should currently follow the object
    public bool cameraFollowEnabled = true;

    [Header("Dead Zone Margins")]
    // How far the object can move from the camera center before the camera follows
    public float horizontalMargin = 2f;
    public float verticalMargin = 2f;

    [Header("Camera Bounds")]
    // Enable or disable camera boundary constraints
    public bool useCameraLimits = true;

    // Minimum and maximum camera positions
    public float minX = -10f;
    public float maxX = 50f;
    public float minY = -5f;
    public float maxY = 10f;

    // Internal reference to the Camera component
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // Called after all Update methods — used for smooth camera movement
    private void LateUpdate()
    {
        // Do nothing if tracking is disabled or no object is assigned
        if (!cameraFollowEnabled || trackedObject == null) return;

        // Store current and target camera positions
        Vector3 currentPos = transform.position;
        Vector3 targetPos = currentPos;

        // Calculate half of the camera's height and width in world units
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.aspect * halfHeight;

        // Calculate the distance from the tracked object to the camera
        Vector2 delta = trackedObject.position - currentPos;

        // Adjust horizontal position if outside margin
        if (delta.x > horizontalMargin)
        {
            targetPos.x += delta.x - horizontalMargin;
        }
        else if (delta.x < -horizontalMargin)
        {
            targetPos.x += delta.x + horizontalMargin;
        }

        // Adjust vertical position if outside margin
        if (delta.y > verticalMargin)
        {
            targetPos.y += delta.y - verticalMargin;
        }
        else if (delta.y < -verticalMargin)
        {
            targetPos.y += delta.y + verticalMargin;
        }

        // Clamp camera position to the defined bounds if limits are enabled
        if (useCameraLimits)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, minX + halfWidth, maxX - halfWidth);
            targetPos.y = Mathf.Clamp(targetPos.y, minY + halfHeight, maxY - halfHeight);
        }

        // Smoothly move the camera from current to target position
        transform.position = Vector3.Lerp(currentPos, targetPos, followSpeed * Time.deltaTime);
    }

    // Draws the camera's dead zone in the scene view when selected
    private void OnDrawGizmosSelected()
    {
        // Only draw if in play mode and dependencies are assigned
        if (!Application.isPlaying || trackedObject == null || cam == null) return;

        // Set gizmo color
        Gizmos.color = Color.yellow;

        // Calculate dimensions of the camera's half view
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.aspect * halfHeight;

        // Center and size of the dead zone area
        Vector3 center = transform.position;
        Vector3 size = new Vector3(horizontalMargin * 2, verticalMargin * 2, 0);

        // Draw a wireframe rectangle representing the dead zone
        Gizmos.DrawWireCube(center, size);
    }
}