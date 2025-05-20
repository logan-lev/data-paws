using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D cat;
    public CoinScript coinScript;
    public BoxCollider2D playerCollide;
    public Transform respawnPoint;
    public PuzzleManager puzzleManager;
    public TreeManager treeManager;

    public AudioSource jumpSFX;
    public AudioSource landingSFX;
    public AudioSource deathSFX;

    public float acceleration = 1f;
    public float jumpForce = 5f;
    public float maxJumpForce = 10f;
    public float speedLimit = 3f;
    public float friction = 2f;
    private float currentJumpForce;
    private Rigidbody2D rb;
    private Vector2 velocity;
    private float inputAxis;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(1.0f, 0.1f);
    public LayerMask groundLayer;

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float maxJumpHeight = 4f;
    public float maxJumpTime = 2f;
    public float ceilingCheckDistance = 0.6f;

    [Header("Wall Check")]
    public float wallCheckDistance = 0.55f;

    [Header("Pickup System")]
    public Transform holdPoint;
    private PickupItem heldItem;
    public PuzzleManagerlvl2 puzzleManagerLvl2;

    private bool grounded;
    private bool isJumpingHeld = false;


    private float gravity;
    private Vector3 startingPosition;

    // Animation
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        jumpForce = 2f * maxJumpHeight / (maxJumpTime / 2f);
        gravity = -1.8f * maxJumpHeight / Mathf.Pow(maxJumpTime / 2f, 2f);
        startingPosition = transform.position;
        Physics2D.IgnoreLayerCollision(0,11,true);
    }

    private void Update()
    {
        inputAxis = Input.GetAxis("Horizontal");

        // Wall check
        bool hittingWallRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, groundLayer);
        bool hittingWallLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, groundLayer);

        if ((velocity.x > 0f && hittingWallRight) || (velocity.x < 0f && hittingWallLeft))
        {
            velocity.x = 0f;
        }

        float targetSpeed = inputAxis * moveSpeed;

        if (Mathf.Abs(inputAxis) > 0.01f)
        {
            float acceleration = grounded ? moveSpeed * 2f : moveSpeed;
            velocity.x = Mathf.MoveTowards(velocity.x, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            float deceleration = grounded ? moveSpeed * 10f : moveSpeed * 4f;
            velocity.x = Mathf.MoveTowards(velocity.x, 0f, deceleration * Time.deltaTime);
        }

        grounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        if (grounded && velocity.y < 0f)
        {
            velocity.y = -0.1f;
        }

        if (grounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
        {
        jumpSFX.Play();
        velocity.y = jumpForce;
        isJumpingHeld = true;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
        {
        isJumpingHeld = false;
        }

        RaycastHit2D ceilingHit = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, groundLayer);
        if (ceilingHit.collider != null && velocity.y > 0f)
        {
            velocity.y = 0f;
        }

        bool isFalling = velocity.y < 0f;
        bool jumpCut = !isJumpingHeld && velocity.y > 0f;

        float gravityMultiplier = isFalling ? 1.5f : (jumpCut ? 2.5f : 1f);

        velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);

        // --- Animator Updates ---
        animator.SetBool("isWalking", Mathf.Abs(inputAxis) > 0.01f && grounded);
        animator.SetBool("isJumping", !grounded);

        // --- Flip Sprite ---
        if (inputAxis > 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (inputAxis < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
            
        if (Input.GetKeyDown(KeyCode.E))
{
    if (heldItem == null)
    {
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
        heldItem.Drop(Vector2.right * transform.localScale.x); // throw direction matches facing
        heldItem = null;
    }
}
    
    }

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
    puzzleManager.ResetPlayerOnly(); // only reset player
}

if (collision.CompareTag("Checkpoint"))
{
    puzzleManager.StartPuzzleCheckpoint(collision.transform); // just start puzzle
}


        if(collision.CompareTag("PlayerIgnore")){
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

public void UpdateRespawnPoint(Vector3 newPosition)
{
    if (respawnPoint != null)
    {
        respawnPoint.position = newPosition;
    }
}
public bool IsHoldingItem()
{
    return heldItem != null;
}



}