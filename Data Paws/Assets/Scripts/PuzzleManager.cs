using UnityEngine;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    public Transform cameraTargetPosition;
    public float cameraZoomSize = 8f;
    public float cameraLerpSpeed = 5f;
    public Camera mainCamera;
    public Transform player;
    public Rock_Hole[] holes;
    public GameObject[] rocks;
    public Transform[] rockStartPositions;

    private Vector3 originalCameraPos;
    private float originalZoom;
    private Vector3 currentCheckpoint;
    private Vector3 startingPosition;
    private int lastFacingDirection = 1;

    private bool puzzleStarted = false;
    private bool puzzleComplete = false;
    private int correctHole = 0;

    [Header("Pickup Settings")]
    public Transform holdPoint;
    private GameObject heldRock;

    [Header("UI")]
    public GameObject pickupPrompt;

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

        if (IsPuzzleSolved())
        {
            puzzleComplete = true;

            if (pickupPrompt != null)
            {
                pickupPrompt.SetActive(false);
            }

            ReturnCameraToPlayer();
        }

        HandleRockPickup();
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
    }

    public void ResetPuzzle()
    {
        if (heldRock != null)
        {
            DropRock();
        }

        // Reset player position and velocity
        player.position = currentCheckpoint;
        Vector3 pos = player.transform.position;
        pos.z = 0f;
        player.transform.position = pos;

        Rigidbody2D prb = player.GetComponent<Rigidbody2D>();
        if (prb) prb.linearVelocity = Vector2.zero;

        // Reset each rock
        for (int i = 0; i < rocks.Length; i++)
        {
            GameObject rock = rocks[i];
            Transform resetPos = rockStartPositions[i];

            rock.transform.position = resetPos.position;
            rock.SetActive(true);

            Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.simulated = true;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            Collider2D col = rock.GetComponent<Collider2D>();
            if (col)
            {
                col.enabled = true;
            }

            Rock_Identifier id = rock.GetComponent<Rock_Identifier>();
            if (id)
            {
                id.isSnapped = false;
            }
        }

        correctHole = 0;
    }

    bool IsPuzzleSolved()
    {

        correctHole = 0;

        foreach (Rock_Hole hole in holes)
        {
            if (hole.isCorrect) 
            {
                correctHole++;
            }
        }
        
        if (correctHole == 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void HandleRockPickup()
    {
    if (puzzleComplete) return;
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            lastFacingDirection = moveInput > 0 ? 1 : -1;
        }

        if (heldRock == null)
        {
            bool nearRock = false;
            Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, 1.5f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Rock"))
                {
                    Rock_Identifier id = hit.GetComponent<Rock_Identifier>();
                    if (id == null || id.isSnapped)
                    {
                        continue;
                    }
            
                    nearRock = true;
                    break;
                }
            }

            if (pickupPrompt != null)
                pickupPrompt.SetActive(nearRock);
        }
        else
        {
            if (pickupPrompt != null)
                pickupPrompt.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldRock == null)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, 1.5f);
                foreach (var hit in hits)
                {
                    if (hit.CompareTag("Rock"))
                    {
                        Rock_Identifier id = hit.GetComponent<Rock_Identifier>();
                        if (id != null && id.isSnapped)
                        {
                            continue;
                        }

                        heldRock = hit.gameObject;

                        float rockHeight = 1f;
                        Collider2D col = heldRock.GetComponent<Collider2D>();
                        if (col != null)
                        {
                            rockHeight = col.bounds.size.y;
                        }

                        Vector3 pickupPos = player.position + new Vector3(0f, 1f + (rockHeight / 2f), 0f);
                        heldRock.transform.position = pickupPos;

                        heldRock.transform.SetParent(holdPoint);

                        Rigidbody2D rb = heldRock.GetComponent<Rigidbody2D>();
                        if (rb) rb.simulated = false;
                        if (col) col.enabled = false;

                        break;
                    }
                }
            }
            else
            {
                DropRock();
            }
        }
    }

    void DropRock()
    {
        heldRock.transform.SetParent(null);

        float rockHeightOffset = 0.5f;
        Collider2D col = heldRock.GetComponent<Collider2D>();
        if (col != null)
        {
            rockHeightOffset = col.bounds.extents.y;
        }

        Vector3 dropOffset = new Vector3(3f * lastFacingDirection, rockHeightOffset, 0f);
        heldRock.transform.position = player.position + dropOffset;

        Rigidbody2D rb = heldRock.GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = true;
        if (col != null) col.enabled = true;

        heldRock = null;
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
