using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SideScrollingCamera : MonoBehaviour
{
    public Transform trackedObject;
    public float followSpeed = 5f;
    public bool cameraFollowEnabled = true;



    [Header("Dead Zone Margins")]
    public float horizontalMargin = 2f;
    public float verticalMargin = 2f;

    [Header("Camera Bounds")]
    public bool useCameraLimits = true;
    public float minX = -10f;
    public float maxX = 50f;
    public float minY = -5f;
    public float maxY = 10f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
{
    if (!cameraFollowEnabled || trackedObject == null) return;

    Vector3 currentPos = transform.position;
    Vector3 targetPos = currentPos;

    float halfHeight = cam.orthographicSize;
    float halfWidth = cam.aspect * halfHeight;

    Vector2 delta = trackedObject.position - currentPos;

    if (delta.x > horizontalMargin)
    {
        targetPos.x += delta.x - horizontalMargin;
    }
    else if (delta.x < -horizontalMargin)
    {
        targetPos.x += delta.x + horizontalMargin;
    }

    if (delta.y > verticalMargin)
    {
        targetPos.y += delta.y - verticalMargin;
    }
    else if (delta.y < -verticalMargin)
    {
        targetPos.y += delta.y + verticalMargin;
    }

    
    if (useCameraLimits)
    {
        targetPos.x = Mathf.Clamp(targetPos.x, minX + halfWidth, maxX - halfWidth);
        targetPos.y = Mathf.Clamp(targetPos.y, minY + halfHeight, maxY - halfHeight);
    }

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