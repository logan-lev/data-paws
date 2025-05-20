using UnityEngine;

// Require a Rigidbody2D component on the GameObject
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // Rigidbody used for movement
    public Rigidbody2D cat;

    // Optional reference to a coin script
    public CoinScript coinScript;

    // Collider for the player
    public BoxCollider2D playerCollide;

    // Respawn position
    public Transform respawnPoint;

    // Reference to PuzzleManager for general puzzles
    public PuzzleManager puzzleManager;

    // Reference to TreeManager for tree-based puzzles
    public TreeManager treeManager;

    // Audio clips for different player actions
    public AudioSource jumpSFX;
    public AudioSource landingSFX;
    public AudioSource deathSFX;

    // Movement and physics values
    public float acceleration = 1f;
    public float jumpForce = 5f;
    public float maxJumpForce = 10f;
    public float speedLimit = 3f;
    public float friction = 2f;

    // Rigidbody reference and velocity tracking
    private Rigidbody2D rb;
    private Vector2 velocity;
    private float inputAxis;

    // Ground detection settings
    [Header("Ground Detection")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(1.0f, 0.1f);
    public LayerMask groundLayer;

    // Movement configuration
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float maxJumpHeight = 4f;
    public float maxJumpTime = 2f;
    public float ceilingCheckDistance = 0.6f;

    // Wall detection settings
    [Header("Wall Check")]
    public float wallCheckDistance = 0.55f;

    // Item pickup settings
    [Header("Pickup System")]
    public Transform holdPoint;
    private PickupItem heldItem;
    public PuzzleManagerlvl2 puzzleManagerLvl2;

    // State flags
    private bool grounded;
    private bool isJumpingHeld = false;

    // Internal physics state
    private float gravity;
    private Vector3 startingPosition;

    // Animator for movement animations
    private Animator animator;

    // Called before Start — initializes rigidbody and animator
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Calculate jump force and gravity based on jump height and time
        jumpForce = 2f * maxJumpHeight / (maxJumpTime / 2f);
        gravity = -1.8f * maxJumpHeight / Mathf.Pow(maxJumpTime / 2f, 2f);

        // Save the initial position
        startingPosition = transform.position;

        // Ignore collisions between specific layers (e.g., player and ignore)
        Physics2D.IgnoreLayerCollision(0,11,true);
    }

    // Called every frame — handles input and movement
    private void Update()
    {
        // Get horizontal input axis
        inputAxis = Input.GetAxis("Horizontal");

        // Check for walls to the left and right
        bool hittingWallRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, groundLayer);
        bool hittingWallLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, groundLayer);

        // Stop horizontal movement if hitting a wall
        if ((velocity.x > 0f && hittingWallRight) || (velocity.x < 0f && hittingWallLeft))
        {
            velocity.x = 0f;
        }

        // Calculate the target horizontal speed
        float targetSpeed = inputAxis * moveSpeed;

        // Accelerate toward the target speed
        if (Mathf.Abs(inputAxis) > 0.01f)
        {
            float acceleration = grounded ? moveSpeed * 2f : moveSpeed;
            velocity.x = Mathf.MoveTowards(velocity.x, targetSpeed, acceleration * Time.deltaTime);
        }
        // Decelerate to zero if no input
        else
        {
            float deceleration = grounded ? moveSpeed * 10f : moveSpeed * 4f;
            velocity.x = Mathf.MoveTowards(velocity.x, 0f, deceleration * Time.deltaTime);
        }

        // Check if the player is on the ground
        grounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        // Prevent the player from accelerating downward when grounded
        if (grounded && velocity.y < 0f)
        {
            velocity.y = -0.1f;
        }

        // Jump input handling
        if (grounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
        {
            jumpSFX.Play();
            velocity.y = jumpForce;
            isJumpingHeld = true;
        }

        // Stop upward acceleration when jump is released
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
        {
            isJumpingHeld = false;
        }

        // Stop upward motion if hitting the ceiling
        RaycastHit2D ceilingHit = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, groundLayer);
        if (ceilingHit.collider != null && velocity.y > 0f)
        {
            velocity.y = 0f;
        }

        // Gravity adjustments
        bool isFalling = velocity.y < 0f;
        bool jumpCut = !isJumpingHeld && velocity.y > 0f;
        float gravityMultiplier = isFalling ? 1.5f : (jumpCut ? 2.5f : 1f);
        velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);

        // Update animator based on movement
        animator.SetBool("isWalking", Mathf.Abs(inputAxis) > 0.01f && grounded);
        animator.SetBool("isJumping", !grounded);

        // Flip character sprite based on direction
        if (inputAxis > 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (inputAxis < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        // Handle item pickup and drop
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
            {
                // Find nearby pickup items
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 2f);
                foreach (var hit in hits)
                {
                    PickupItem item = hit.GetComponent<PickupItem>();
                    if (item != null)
                    {
                        heldItem = item;
                        item.PickUp(holdPoint);

                        if (puzzleManagerLvl2 != null)
                            puzzleManagerLvl2.UpdateCurrentItemName(item.GetItemName());

                        break;
                    }
                }
            }
            else
            {
                // Drop the currently held item
                heldItem.Drop(Vector2.right * transform.localScale.x);
                heldItem = null;
            }
        }
    }

    // Called at fixed intervals — applies velocity to the Rigidbody
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        Physics2D.IgnoreLayerCollision(0,11);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle") || collision.CompareTag("Enemy"))
        {
            deathSFX.Play();
            Respawn();
        }

        if (collision.CompareTag("River"))
        {
            deathSFX.Play();
            puzzleManager.ResetPlayerOnly();
        }

        if (collision.CompareTag("Checkpoint"))
        {
            puzzleManager.StartPuzzleCheckpoint(collision.transform);
        }

        if(collision.CompareTag("PlayerIgnore"))
        {
            Physics2D.IgnoreCollision(collision, playerCollide,true);
        }

        if (collision.CompareTag("Exit"))
        {
            puzzleManager.ReturnCameraToPlayer();

            Collider2D col = collision.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }

    // Draws debug gizmos in the editor for ground and wall checks
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallCheckDistance);
    }

    // Respawn logic for different puzzle managers or manual respawn point
    void Respawn()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        if (treeManager != null)
        {
            treeManager.RespawnAtCheckpoint();
        }
        else if (puzzleManagerLvl2 != null)
        {
            puzzleManagerLvl2.ResetPlayerToLevelStart();
        }
        else if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
    }

    // Updates the respawn position to a new location
    public void UpdateRespawnPoint(Vector3 newPosition)
    {
        if (respawnPoint != null)
        {
            respawnPoint.position = newPosition;
        }
    }

    // Returns true if the player is currently holding an item
    public bool IsHoldingItem()
    {
        return heldItem != null;
    }
}