using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanController : MonoBehaviour
{
    public GameObject ball;

    private Rigidbody ballRigidBody;
    private float movementSpeed;



    private void Awake()
    {
        ballRigidBody = ball.GetComponent<Rigidbody>();
        movementSpeed = FindObjectOfType<LevelController>().movementSpeed;
    }
    void Start()
    {

    }

    private void FixedUpdate()
    {
        fanMovement();
        blowAir();
    }

    private void fanMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 movement = new Vector3(touch.deltaPosition.x * Time.fixedDeltaTime * movementSpeed, 0, 0);
                transform.Translate(movement);
            }
        }
    }
    
    private void blowAir()
    {
        if (Physics.Raycast(ballRigidBody.position, Vector3.down, 10))
        {
            resetBallVelocity();
            ballRigidBody.useGravity = false;
            Vector3 fanToBallVector = Vector3.Normalize(ballRigidBody.position - transform.position);
            ballRigidBody.AddForceAtPosition(fanToBallVector * 5, transform.position);
        }
        else
            ballRigidBody.useGravity = true;
    }

    private void resetBallVelocity()
    {
        if (ballRigidBody.velocity.y > 0)
            return;

        Vector3 velocity = ballRigidBody.velocity;
        velocity.y = 0;
        ballRigidBody.velocity = velocity;
    }
}
