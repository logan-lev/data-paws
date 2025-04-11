using UnityEngine;

public class playerController : MonoBehaviour
{
    public Rigidbody2D cat;
    public float acceleration = 1f;
    public float jumpForce = 5f;
    public float maxJumpForce = 10f;
    public float speedLimit = 3f;
    public float friction = 2f;
    private bool canJump = true;
    private float currentJumpForce;
    public LogicScript logic;

    private bool restart = false;
    public GameObject respawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && canJump)
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.Space) && canJump)
        {
            currentJumpForce = jumpForce;
            jumpForce = maxJumpForce;
            Jump();
            jumpForce = currentJumpForce;
        }
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
            MoveLeft();
        }
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            MoveRight();
        }
        if((Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A)) && canJump){
            cat.linearVelocity = Vector2.zero;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            cat.linearVelocity = Vector2.zero;
            cat.transform.position = respawnPoint.transform.position;
        }
    }

    void MoveRight()
    {
        if(cat.linearVelocityX < speedLimit)
        {
            cat.linearVelocityX += acceleration;
        }   
    }

    void MoveLeft()
    {
        if(cat.linearVelocityX > -speedLimit)
        {
            cat.linearVelocityX -= acceleration;
        }
    }

    // void StopMove(){
    //     if(cat.linearVelocityX > 0)
    //     {
    //         cat.linearVelocityX -= friction;
    //     } 
    //     if(cat.linearVelocityX < 0)
    //     {
    //         cat.linearVelocityX += friction;
    //     }
    // }

    void Jump()
    {
        cat.linearVelocity = Vector2.up*jumpForce;
        canJump = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy"){
            cat.linearVelocity = Vector2.zero;
            cat.transform.position = respawnPoint.transform.position;
        }
        canJump = true;
    }
}
