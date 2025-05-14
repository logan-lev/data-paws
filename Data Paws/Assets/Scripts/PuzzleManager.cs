using UnityEngine;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    public float cameraLerpSpeed = 5f;
    public Camera mainCamera;
    public Transform player;
    public Collider2D playerCollider;
    public Rock_Hole[] holes;
    public GameObject[] rocks;
    public Transform[] rockStartPositions;
    [Header("Pause Settings")]
public GameObject pausePanel;
private bool isPaused = false;

   
    public Camera zoomedOutCamera;

    [Header("Environment Control")]
    public GameObject invisibleWall;
    public GameObject invisibleDoor;

    [Header("Puzzle Barriers")]
    public GameObject[] RiverCollisions;



    public AudioSource puzzleFailSFX;
    public AudioSource puzzlePassSFX;

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

        pausePanel.SetActive(false);
    isPaused = false;
    Time.timeScale = 1f;

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
    if (!puzzleComplete && Input.GetKeyDown(KeyCode.R))
    {
        ResetPuzzle();
    }

    HandleRockPickup();

    if (Input.GetKeyDown(KeyCode.P))
{
    TogglePause();
}
}

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

    // Optionally disable main camera follow
    SideScrollingCamera camFollow = mainCamera.GetComponent<SideScrollingCamera>();
    if (camFollow != null)
        camFollow.cameraFollowEnabled = false;
}

   public void ResetPuzzle()
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
public void OnCorrectPlacement()
{
    correctHole++;

    if (correctHole >= holes.Length && !puzzleComplete)
    {
        PuzzleCompleted();
    }
}
public void TogglePause()
{
    isPaused = !isPaused;
    pausePanel.SetActive(isPaused);
    Time.timeScale = isPaused ? 0f : 1f;
}
}
