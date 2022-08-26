using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanController : MonoBehaviour
{
    public GameObject ball;

    private Rigidbody ballRigidBody;
    private float touchSensivity;
    private Vector3 blowPower;
    private float movementSpeed;
    private int ballToFanLayerMask;
    private void Awake()
    {
        LevelController levelController = FindObjectOfType<LevelController>();
        ballRigidBody = ball.GetComponent<Rigidbody>();
        touchSensivity = levelController.touchSensivity;
        blowPower = levelController.blowPower;
        movementSpeed = levelController.movementSpeed;
        ballToFanLayerMask = LayerMask.GetMask("Default", "Fan");
    }
    private void FixedUpdate()
    {
        moveFanAndPlayer();
        fanMovement();
        blowAir();
    }

    private void moveFanAndPlayer()
    {
        Vector3 destination = ballRigidBody.position;
        destination.z += movementSpeed * Time.fixedDeltaTime;

        ballRigidBody.MovePosition(destination);
        transform.Translate(new Vector3(0, 0, movementSpeed * Time.fixedDeltaTime));
    }

    private void fanMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 movement = new Vector3(touch.deltaPosition.x * Time.fixedDeltaTime * touchSensivity, 0, 0);
                if (Physics.Raycast(transform.position + movement, Vector3.down))
                    transform.Translate(movement);
            }
        }
    }
    
    private void blowAir()
    {
        if (Input.touchCount == 0)
            return;

        if(ballRigidBody.position.y > 5 && ballRigidBody.velocity.y > 0)
        {
            Vector3 velocity = ballRigidBody.velocity;
            velocity.y -= 5 * Time.fixedDeltaTime;
            ballRigidBody.velocity = velocity;
            return;

        }
        
        if (Physics.SphereCast(ballRigidBody.position, 1, Vector3.down, out RaycastHit a, 5f, ballToFanLayerMask))
        {
            Vector3 velocity = calculateVelocity();
            ballRigidBody.AddForceAtPosition(velocity * 3, transform.position);
        }
    }

    private Vector3 calculateVelocity()
    {
        Vector3 velocity = Vector3.Normalize(ballRigidBody.position - transform.position);
        velocity.z = 0;
        velocity.x *= blowPower.x;
        velocity.y *= blowPower.y;

        if (ballRigidBody.velocity.y < 0)
            velocity.y -= ballRigidBody.velocity.y;

        if ((transform.position.x < ballRigidBody.position.x && velocity.x < 0) || (transform.position.x > ballRigidBody.position.x && velocity.x > 0))
            velocity.x *= 2;
        
        return velocity;
        

    }
}
