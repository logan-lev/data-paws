using UnityEngine;

public class playerController : MonoBehaviour
{
    public Rigidbody2D cat;
    public float acceleration = 0.5f;
    public float jumpForce = 5f;
    public float speedLimit = 3f;
    private bool canJump = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) && canJump){
            Jump();
        }
        if(Input.GetKey(KeyCode.LeftArrow)){
            MoveLeft();
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            MoveRight();
        }
        if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)){
            StopMove();
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

    void StopMove(){
        cat.linearVelocityX = 0;
    }

    void Jump()
    {
        cat.linearVelocity = Vector2.up*jumpForce;
        canJump = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
    }
}
