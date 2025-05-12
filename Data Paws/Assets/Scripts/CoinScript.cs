using UnityEngine;

public class CoinScript : MonoBehaviour
{

    public GameObject coinObject;
    public Rigidbody2D myRigidBody;
    public GameObject respawnPoint;
    public SpriteRenderer coinSprite;

    public bool isMovingRight;
    // public float runSpeed;

    void Start()
    {
        // myRigidBody.linearVelocity = UnityEngine.Vector2.right*runSpeed;
        isMovingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }
        // myRigidBody.AddForceX(runSpeed);
     if(myRigidBody.linearVelocityX == 0){
            if(isMovingRight){
                // myRigidBody.linearVelocity = UnityEngine.Vector2.left*runSpeed;        
            }
            else{
                // myRigidBody.linearVelocity = UnityEngine.Vector2.right*runSpeed;
            }
            isMovingRight = !isMovingRight;
            
        }
    }

    public void ResetPosition(){
        myRigidBody.transform.position = respawnPoint.transform.position;
        // myRigidBody.linearVelocity = UnityEngine.Vector2.right*runSpeed;
        coinObject.SetActive(true);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        coinObject.SetActive(false);
    }
}
