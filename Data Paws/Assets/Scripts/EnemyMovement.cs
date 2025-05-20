using UnityEngine;

// Controls enemy movement, direction switching, and reset behavior
public class EnemyMovement : MonoBehaviour
{
    // Rigidbody component used for movement
    public Rigidbody2D myRigidBody;

    // Reference point where the enemy respawns
    public GameObject respawnPoint;

    // SpriteRenderer to visually flip the enemy
    public SpriteRenderer enemySprite;

    // Tracks if the enemy is currently moving right
    public bool isMovingRight;

    // Movement speed of the enemy
    public float runSpeed;

    // Animator component for playing animations
    private Animator animator;
    
    void Start()
    {
        // Set initial movement to the right
        myRigidBody.linearVelocity = Vector2.right*runSpeed;

        // Mark direction as moving right
        isMovingRight = true;

        // Get the Animator component and play the walking animation
        animator = GetComponent<Animator>();
        animator.Play("Enemy_Walk");
    }

    void Update()
    {
        // Pressing 'R' will reset the enemy's position
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }

        // If the enemy's movement has stopped (e.g., hit a wall), reverse direction
        if(myRigidBody.linearVelocityX == 0)
        {
            // If moving right, reverse to left
            if(isMovingRight)
            {
                myRigidBody.linearVelocity = Vector2.left*runSpeed;        
            }
            // If moving left, reverse to right
            else
            {
                myRigidBody.linearVelocity = Vector2.right*runSpeed;
            }

            // Flip the sprite and update the direction flag
            enemySprite.flipX = !enemySprite.flipX;
            isMovingRight = !isMovingRight;
        }
    }

    // Resets the enemy's position and movement
    public void ResetPosition()
    {
        myRigidBody.transform.position = respawnPoint.transform.position;
        myRigidBody.linearVelocity = Vector2.right*runSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the enemy is no longer moving, reverse direction
        if(myRigidBody.linearVelocityX == 0)
        {
            // Reverse from right to left
            if(isMovingRight)
            {
                myRigidBody.linearVelocity = Vector2.left*runSpeed;        
            }
            // Reverse from left to right
            else
            {
                myRigidBody.linearVelocity = Vector2.right*runSpeed;
            }

            // Flip sprite and direction state
            enemySprite.flipX = !enemySprite.flipX;
            isMovingRight = !isMovingRight;
        }
    }
}