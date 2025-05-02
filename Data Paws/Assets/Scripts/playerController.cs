using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 16f;

    [Header("Ground Check Settings")]
    public Transform groundCheckPoint;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    public LayerMask groundLayer;

    [Header("Wall Check Settings")]
    public Transform wallCheckPoint;
    private Vector2 wallCheckSize = new Vector2(2f, 1f);

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool touchingWall;
    private float moveInput;

    public Transform respawnPoint;
    public PuzzleManager puzzleManager;

    public AudioSource jumpSFX;
    public AudioSource landingSFX;
    public AudioSource deathSFX;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // --- Get Horizontal Input from Keys ---
        moveInput = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveInput = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveInput = 1f;

        // --- Jump Input ---
        if (isGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpSFX.Play();
        }

        animator.SetBool("isWalking", moveInput != 0 && isGrounded);
        animator.SetBool("isJumping", !isGrounded);

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f); // Facing right
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); // Facing left
        }

    }

    void FixedUpdate()
    {
        // --- Move Player ---
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // --- Ground Check ---
        isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0f, groundLayer);

        // --- Wall Check ---
        touchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0f, groundLayer);

        // --- Fix sticking to wall ---
        if (touchingWall && !isGrounded)
        {
            if (rb.linearVelocity.y > 0)
            {
                // If going up into a wall, stop vertical velocity a little
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
            else
            {
                // If sliding down against wall, fall faster
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -3f);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
        }
        if (wallCheckPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(wallCheckPoint.position, wallCheckSize);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Respawn();
            deathSFX.Play();
        }

        if (collision.CompareTag("Ground"))
        {
            landingSFX.Play();

        }

        if (collision.CompareTag("Checkpoint"))
        {
            puzzleManager.ResetPuzzle();
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

    void Respawn()
    {
        rb.linearVelocity = Vector2.zero; // Cancel current motion
        transform.position = respawnPoint.position; // Teleport to respawn
    }
}