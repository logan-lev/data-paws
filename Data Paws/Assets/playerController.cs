using UnityEngine;

public class playerController : MonoBehaviour
{
    public Rigidbody2D cat;
    public float moveSpeed = 2f;
    public float acceleration = 1f;
    public float jumpForce = 5f;
    public float speedLimit = 8f;
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
    }

    void MoveRight()
    {
        //cat.linearVelocity = Vector2.right*moveSpeed;

        cat.linearVelocityX += 0.5f;
    }

    void MoveLeft()
    {
        //cat.linearVelocity = Vector2.left*moveSpeed;

        cat.linearVelocityX -= 0.5f;
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
