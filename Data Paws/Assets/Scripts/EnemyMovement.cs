using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D myRigidBody;
    public GameObject respawnPoint;
    public SpriteRenderer enemySprite;
    public bool isMovingRight;
    public float runSpeed;
    void Start()
    {
        myRigidBody.linearVelocity = UnityEngine.Vector2.right*runSpeed;
        isMovingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }
        myRigidBody.AddForceX(runSpeed);
     if(myRigidBody.linearVelocityX == 0){
            if(isMovingRight){
                myRigidBody.linearVelocity = UnityEngine.Vector2.left*runSpeed;        
            }
            else{
                myRigidBody.linearVelocity = UnityEngine.Vector2.right*runSpeed;
            }
            enemySprite.flipX = !enemySprite.flipX;
            isMovingRight = !isMovingRight;
            
        }
    }

    void ResetPosition(){
        myRigidBody.transform.position = respawnPoint.transform.position;
        myRigidBody.linearVelocity = UnityEngine.Vector2.right*runSpeed;
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(myRigidBody.linearVelocityX == 0){
            if(isMovingRight){
                myRigidBody.linearVelocity = UnityEngine.Vector2.left*runSpeed;        
            }
            else{
                myRigidBody.linearVelocity = UnityEngine.Vector2.right*runSpeed;
            }
            enemySprite.flipX = !enemySprite.flipX;
            isMovingRight = !isMovingRight;
            
        }
    }
}
