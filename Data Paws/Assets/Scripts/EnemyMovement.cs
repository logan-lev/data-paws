using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D myRigidBody;
    public float runSpeed;
    void Start()
    {
        myRigidBody.linearVelocity = UnityEngine.Vector2.right*runSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
