using UnityEngine;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    public Transform cameraTargetPosition;
    public float cameraZoomSize = 8f;
    public float cameraLerpSpeed = 5f;
    public Camera mainCamera;
    public Transform player;
    public GameObject door;
    public BoxHole[] holes;
    public GameObject[] boxes;
    public Transform[] boxStartPositions;

    private Vector3 originalCameraPos;
    private float originalZoom;
    private Vector3 currentCheckpoint;
    private Vector3 startingPosition;
    private int lastFacingDirection = 1;

    private bool puzzleStarted = false;
    private bool puzzleComplete = false;

    [Header("Pickup Settings")]
    public Transform holdPoint;
    private GameObject heldBox;

    [Header("UI")]
    public GameObject pickupPrompt;

    [Header("Puzzle Barrier")]
    public GameObject puzzleBarrier;

    void Start()
    {
        originalCameraPos = mainCamera.transform.position;
        originalZoom = mainCamera.orthographicSize;

        startingPosition = player.position;         
        currentCheckpoint = startingPosition;   
    }

    void Update()
    {
        if (!puzzleComplete && Input.GetKeyDown(KeyCode.R))
        {
            ResetPuzzle();
        }

        if (puzzleStarted && !puzzleComplete && IsPuzzleSolved())
{
    puzzleComplete = true;
    door.SetActive(false);

    if (pickupPrompt != null)
        pickupPrompt.SetActive(false);

    if (puzzleBarrier != null)
        puzzleBarrier.SetActive(false);

    ReturnCameraToPlayer();
}

        HandleBoxPickup();
    }

    public void StartPuzzleCheckpoint(Transform checkpoint)
{
    if (puzzleStarted || puzzleComplete)
        return;

    puzzleStarted = true;
    currentCheckpoint = checkpoint.position;

    if (mainCamera != null)
    {
        SideScrollingCamera camFollow = mainCamera.GetComponent<SideScrollingCamera>();
        if (camFollow != null)
            camFollow.cameraFollowEnabled = false;

        mainCamera.transform.position = cameraTargetPosition.position;
        mainCamera.orthographicSize = cameraZoomSize;
    }

    if (puzzleBarrier != null)
        puzzleBarrier.SetActive(true);
}

    void ResetPuzzle()
    {
        if (heldBox != null)
        {
            DropBox();
        }

        player.position = currentCheckpoint;
        Vector3 pos = player.transform.position;
        pos.z = 0f;
        player.transform.position = pos;

        Rigidbody2D prb = player.GetComponent<Rigidbody2D>();
        if (prb) prb.linearVelocity = Vector2.zero;

        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].transform.position = boxStartPositions[i].position;
            boxes[i].SetActive(true);

            Rigidbody2D rb = boxes[i].GetComponent<Rigidbody2D>();
            if (rb) rb.linearVelocity = Vector2.zero;
        }
    }

    bool IsPuzzleSolved()
    {
        foreach (BoxHole hole in holes)
        {
            if (!hole.isCorrect) return false;
        }
        return true;
    }

    void HandleBoxPickup()
{
    if (puzzleComplete) return;
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            lastFacingDirection = moveInput > 0 ? 1 : -1;
        }

        if (heldBox == null)
        {
            bool nearBox = false;
            Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, 1.5f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Box"))
                {
                    nearBox = true;
                    break;
                }
            }

            if (pickupPrompt != null)
                pickupPrompt.SetActive(nearBox);
        }
        else
        {
            if (pickupPrompt != null)
                pickupPrompt.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldBox == null)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, 1.5f);
                foreach (var hit in hits)
                {
                    if (hit.CompareTag("Box"))
                    {
                        heldBox = hit.gameObject;

                        float boxHeight = 1f;
                        Collider2D col = heldBox.GetComponent<Collider2D>();
                        if (col != null)
                        {
                            boxHeight = col.bounds.size.y;
                        }

                        Vector3 pickupPos = player.position + new Vector3(0f, 1f + (boxHeight / 2f), 0f);
                        heldBox.transform.position = pickupPos;

                        heldBox.transform.SetParent(holdPoint);

                        Rigidbody2D rb = heldBox.GetComponent<Rigidbody2D>();
                        if (rb) rb.simulated = false;
                        if (col) col.enabled = false;

                        break;
                    }
                }
            }
            else
            {
                DropBox();
            }
        }
    }

    void DropBox()
    {
        heldBox.transform.SetParent(null);

        float boxHeightOffset = 0.5f;
        Collider2D col = heldBox.GetComponent<Collider2D>();
        if (col != null)
        {
            boxHeightOffset = col.bounds.extents.y;
        }

        Vector3 dropOffset = new Vector3(1.5f * lastFacingDirection, boxHeightOffset, 0f);
        heldBox.transform.position = player.position + dropOffset;

        Rigidbody2D rb = heldBox.GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = true;
        if (col != null) col.enabled = true;

        heldBox = null;
    }

    public void ReturnCameraToPlayer()
{
    if (mainCamera != null)
    {
        mainCamera.transform.position = originalCameraPos;
        mainCamera.orthographicSize = originalZoom;

        SideScrollingCamera camFollow = mainCamera.GetComponent<SideScrollingCamera>();
        if (camFollow != null)
            camFollow.cameraFollowEnabled = true;
    }
}

}
