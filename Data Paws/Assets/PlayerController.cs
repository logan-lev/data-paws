using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    private Vector2 input;
    private bool isMoving;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (input != Vector2.zero)
        {
            isMoving = true;
            MovePlayer(input);
        }
        else
        {
            isMoving = false; // Stop movement when no input
        }
    }

    // Method to move the player based on input
    private void MovePlayer(Vector2 direction)
    {
        // Update the player's position based on input and moveSpeed
        Vector3 targetPos = transform.position;
        targetPos.x += direction.x * moveSpeed * Time.deltaTime;
        targetPos.y += direction.y * moveSpeed * Time.deltaTime;

        // Apply the new position
        transform.position = targetPos;
    }
}