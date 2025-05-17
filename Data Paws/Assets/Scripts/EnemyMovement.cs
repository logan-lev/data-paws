using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D myRigidBody;
    public GameObject respawnPoint;
    public SpriteRenderer enemySprite;
    public bool isMovingRight;
    public float runSpeed;

    private Animator animator;
    
    void Start()
    {
        myRigidBody.linearVelocity = Vector2.right*runSpeed;
        isMovingRight = true;

        animator = GetComponent<Animator>();
        animator.Play("Enemy_Walk");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }

     if(myRigidBody.linearVelocityX == 0)
     {
            if(isMovingRight)
            {
                myRigidBody.linearVelocity = Vector2.left*runSpeed;        
            }
            else
            {
                myRigidBody.linearVelocity = Vector2.right*runSpeed;
            }

            enemySprite.flipX = !enemySprite.flipX;
            isMovingRight = !isMovingRight;
        }
    }

    public void ResetPosition()
    {
        myRigidBody.transform.position = respawnPoint.transform.position;
        myRigidBody.linearVelocity = Vector2.right*runSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(myRigidBody.linearVelocityX == 0)
        {
            if(isMovingRight)
            {
                myRigidBody.linearVelocity = Vector2.left*runSpeed;        
            }
            else
            {
                myRigidBody.linearVelocity = Vector2.right*runSpeed;
            }

            enemySprite.flipX = !enemySprite.flipX;
            isMovingRight = !isMovingRight;
        }
    }
}
