using UnityEngine;
using System.Collections;

// Manages rock puzzle logic, camera control, player interactions, and reset/pause functionality
public class PuzzleManager : MonoBehaviour
{
    // Camera movement speed for lerping
    public float cameraLerpSpeed = 5f;

    // Main camera reference
    public Camera mainCamera;

    // Player transform and collider
    public Transform player;
    public Collider2D playerCollider;

    // References to puzzle hole triggers and rock objects
    public Rock_Hole[] holes;
    public GameObject[] rocks;
    public Transform[] rockStartPositions;

    [Header("Pause Settings")]
    // UI panel shown when game is paused
    public GameObject pausePanel;

    // Whether the game is currently paused
    private bool isPaused = false;

    // Zoomed-out camera for puzzle view
    public Camera zoomedOutCamera;

    [Header("Environment Control")]
    // Barriers to block the player from leaving the puzzle area
    public GameObject invisibleWall;
    public GameObject invisibleDoor;

    [Header("Puzzle Barriers")]
    // River colliders to disable when puzzle is completed
    public GameObject[] RiverCollisions;

    // Audio clips for puzzle success/failure
    public AudioSource puzzleFailSFX;
    public AudioSource puzzlePassSFX;

    // Camera and player state tracking
    private Vector3 originalCameraPos;
    private float originalZoom;
    private Vector3 currentCheckpoint;
    private Vector3 startingPosition;
    private int lastFacingDirection = 1;

    // Puzzle state flags
    private bool puzzleStarted = false;
    private bool puzzleComplete = false;
    private int correctHole = 0;

    [Header("Pickup Settings")]
    // Transform for holding the picked-up rock
    public Transform holdPoint;
    private GameObject heldRock;

    [Header("UI")]
    // UI prompts for interacting with rocks
    public GameObject pickupPrompt;
    public GameObject pickupBackground;

    void Start()
    {
        originalCameraPos = mainCamera.transform.position;
        originalZoom = mainCamera.orthographicSize;

        startingPosition = player.position;
        currentCheckpoint = startingPosition;

        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

        // Disable collisions between player and rocks
        foreach (GameObject rock in rocks)
        {
            Collider2D rockCol = rock.GetComponent<Collider2D>();
            if (rockCol != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, rockCol, true);
            }
        }
    }

    void Update()
    {
        // Full puzzle reset if 'R' is pressed
        if (!puzzleComplete && Input.GetKeyDown(KeyCode.R))
        {
            ResetEverything();
        }

        // Handle rock pickup/drop logic
        HandleRockPickup();

        // Toggle pause if 'P' is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    // Fully resets the puzzle, rocks, player, and holes
    public void ResetEverything()
    {
        if (heldRock != null)
        {
            DropRock();
        }

        // Reset player position and velocity
        puzzleFailSFX.Play();
        player.position = currentCheckpoint;
        Vector3 pos = player.transform.position;
        pos.z = 0f;
        player.transform.position = pos;

        Rigidbody2D prb = player.GetComponent<Rigidbody2D>();
        if (prb) prb.linearVelocity = Vector2.zero;

        // Reset rocks to their start positions
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

                if (playerCollider != null)
                {
                    Physics2D.IgnoreCollision(playerCollider, col, true);
                }
            }

            rock.layer = LayerMask.NameToLayer("Rock");

            Rock_Identifier id = rock.GetComponent<Rock_Identifier>();
            if (id != null)
            {
                id.isSnapped = false;
            }
        }

        // Reset all holes
        foreach (Rock_Hole hole in holes)
        {
            if (hole != null)
            {
                hole.isCorrect = false;
            }
        }

        correctHole = 0;
    }

    // Only resets the player position (not the rocks or holes)
    public void ResetPlayerOnly()
    {
        if (heldRock != null)
        {
            DropRock();
        }

        puzzleFailSFX.Play();
        player.position = currentCheckpoint;
        Vector3 pos = player.transform.position;
        pos.z = 0f;
        player.transform.position = pos;

        Rigidbody2D prb = player.GetComponent<Rigidbody2D>();
        if (prb) prb.linearVelocity = Vector2.zero;
    }

    // Handles logic for picking up and dropping rocks
    void HandleRockPickup()
    {
        if (puzzleComplete) return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            lastFacingDirection = moveInput > 0 ? 1 : -1;
        }

        // Show UI if near a rock
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

            if (pickupBackground != null)
                pickupBackground.SetActive(nearRock);
        }
        else
        {
            if (pickupPrompt != null)
                pickupPrompt.SetActive(false);

            if (pickupBackground != null)
                pickupBackground.SetActive(false);
        }

        // Interact key pressed
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

                        SpriteRenderer sr = heldRock.GetComponent<SpriteRenderer>();
                        if (sr != null)
                        {
                            sr.sortingOrder = 10;
                        }

                        float rockHeight = 1f;
                        Collider2D col = heldRock.GetComponent<Collider2D>();
                        if (col != null)
                        {
                            rockHeight = col.bounds.size.y;
                        }

                        if (playerCollider != null && col != null)
                        {
                            Physics2D.IgnoreCollision(playerCollider, col, true);
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

    // Called to start the puzzle when player enters checkpoint
    public void StartPuzzleCheckpoint(Transform checkpoint)
    {
        if (puzzleStarted || puzzleComplete)
            return;

        puzzleStarted = true;
        currentCheckpoint = checkpoint.position;

        if (invisibleWall != null)
            invisibleWall.SetActive(true);

        if (invisibleDoor != null)
            invisibleDoor.SetActive(true);

        if (mainCamera != null)
            mainCamera.enabled = false;

        if (zoomedOutCamera != null)
        {
            zoomedOutCamera.enabled = true;
            zoomedOutCamera.gameObject.SetActive(true);
        }

        if (mainCamera != null)
        {
            mainCamera.enabled = false;
            mainCamera.gameObject.SetActive(false);
        }

        SideScrollingCamera camFollow = mainCamera.GetComponent<SideScrollingCamera>();
        if (camFollow != null)
            camFollow.cameraFollowEnabled = false;
    }

    // Handles dropping the held rock at the player's position
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
        Vector3 dropPosition = player.position + dropOffset;
        dropPosition.z = 10f;
        heldRock.transform.position = dropPosition;

        Rigidbody2D rb = heldRock.GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = true;
        if (col != null) col.enabled = true;

        if (playerCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, col, true);
        }

        SpriteRenderer sr = heldRock.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 0;
        }

        heldRock = null;
    }

    // Returns camera to player view after puzzle completion
    public void ReturnCameraToPlayer()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(
                player.position.x,
                player.position.y + 2f,
                mainCamera.transform.position.z
            );

            mainCamera.enabled = true;
            mainCamera.gameObject.SetActive(true);
        }

        if (zoomedOutCamera != null)
        {
            zoomedOutCamera.enabled = false;
            zoomedOutCamera.gameObject.SetActive(false);
        }

        SideScrollingCamera camFollow = mainCamera.GetComponent<SideScrollingCamera>();
        if (camFollow != null)
            camFollow.cameraFollowEnabled = true;
    }

    // Marks the puzzle as completed and disables barriers
    public void PuzzleCompleted()
    {
        if (puzzleComplete) return;

        puzzleComplete = true;

        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);

        if (invisibleDoor != null)
            invisibleDoor.SetActive(false);

        foreach (GameObject river in RiverCollisions)
        {
            if (river != null)
            {
                Collider2D col = river.GetComponent<Collider2D>();
                if (col != null)
                    col.enabled = false;
            }
        }

        ReturnCameraToPlayer();
    }

    // Called when a correct rock placement is made
    public void OnCorrectPlacement()
    {
        correctHole++;

        if (correctHole >= holes.Length && !puzzleComplete)
        {
            PuzzleCompleted();
        }
    }

    // Toggles the game's pause state
    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    // Updates the player's current puzzle checkpoint
    public void UpdateCheckpoint(Vector3 newPos)
    {
        currentCheckpoint = newPos;
    }
}