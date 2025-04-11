using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 velocity;
    private float inputAxis;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(1.0f, 0.1f);
    public LayerMask groundLayer;

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float ceilingCheckDistance = 0.6f;

    [Header("Wall Check")]
    public float wallCheckDistance = 0.55f;

    private bool grounded;

    private float jumpForce;
    private float gravity;
    private Vector3 startingPosition;
    private bool onLadder = false;
    public float climbSpeed = 3f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        jumpForce = 2f * maxJumpHeight / (maxJumpTime / 2f);
        gravity = -2f * maxJumpHeight / Mathf.Pow(maxJumpTime / 2f, 2f);
        startingPosition = transform.position;
    }

    private void Update()
    {
        inputAxis = Input.GetAxis("Horizontal");

        // Wall check
        bool hittingWallRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, groundLayer);
        bool hittingWallLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, groundLayer);

        // Block velocity if hitting wall
        if ((velocity.x > 0f && hittingWallRight) || (velocity.x < 0f && hittingWallLeft))
        {
            velocity.x = 0f;
        }

        // Horizontal movement
        float targetSpeed = inputAxis * moveSpeed;

        if (Mathf.Abs(inputAxis) > 0.01f)
        {
            float acceleration = grounded ? moveSpeed * 2f : moveSpeed;
            velocity.x = Mathf.MoveTowards(velocity.x, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // Stronger deceleration when idle
            float deceleration = grounded ? moveSpeed * 10f : moveSpeed * 4f;
            velocity.x = Mathf.MoveTowards(velocity.x, 0f, deceleration * Time.deltaTime);
        }

        // Ground check
        grounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        // Snap to ground
        if (grounded && velocity.y < 0f)
        {
            velocity.y = -0.1f;
        }

        // Jump
        if (grounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            velocity.y = jumpForce;
        }

        // Ceiling check
        RaycastHit2D ceilingHit = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, groundLayer);
        if (ceilingHit.collider != null && velocity.y > 0f)
        {
            velocity.y = 0f;
        }

        // Gravity
        bool isFalling = velocity.y < 0f || !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W));
        float gravityMultiplier = isFalling ? 2f : 1f;
        velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    
    if (Input.GetKeyDown(KeyCode.R))
    {
        ResetPlayerPosition();
    }

    if (onLadder)
{
    rb.gravityScale = 0f;
    velocity.y = Input.GetAxisRaw("Vertical") * climbSpeed;
}
else
{
    rb.gravityScale = 1f;
}
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void ResetPlayerPosition()
{
    transform.position = startingPosition;
    velocity = Vector2.zero;
    rb.linearVelocity = Vector2.zero;
}
private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Ladder"))
        onLadder = true;
}

private void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Ladder"))
        onLadder = false;
}

}