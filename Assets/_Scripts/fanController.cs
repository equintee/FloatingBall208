using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class fanController : MonoBehaviour
{
    public GameObject ball;
    public GameObject gunModel;

    private Rigidbody ballRigidBody;
    private float touchSensivity;
    private Vector3 blowPower;
    private float movementSpeed;
    private int platformLayer;
    private int fanLayer;
    private PathCreator pathCreator;
    private float distanceTravelled = 0f;
    private float initialHeight;
    private void Awake()
    {
        LevelController levelController = FindObjectOfType<LevelController>();

        ballRigidBody = ball.GetComponent<Rigidbody>();
        initialHeight = transform.position.y;

        touchSensivity = levelController.touchSensivity;
        blowPower = levelController.blowPower;
        movementSpeed = levelController.movementSpeed;
        platformLayer = LayerMask.GetMask("Platform");
        fanLayer = LayerMask.GetMask("Fan");
        pathCreator = FindObjectOfType<PathCreator>();
    }
    private void FixedUpdate()
    {
        moveFanAndPlayer();
        fanMovement();
        blowAir();
        checkIfGameFinished();
    }

    private void checkIfGameFinished()
    {
        if(pathCreator.path.GetClosestTimeOnPath(transform.position) == 1f)
        {
            FindObjectOfType<LevelController>().endGame(true);
            return;
        }

        if(ballRigidBody.transform.position.y < initialHeight - 4f)
        {
            FindObjectOfType<LevelController>().endGame(false);
            return;
        }
            
    }

    private void moveFanAndPlayer()
    {
        distanceTravelled += movementSpeed * Time.fixedDeltaTime;
        Vector3 destination = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);

        destination.y = initialHeight;

        ballRigidBody.velocity = Vector3.Scale(ballRigidBody.velocity, transform.right) + new Vector3(0, ballRigidBody.velocity.y, 0);

        transform.position = destination;
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Stop);

        if (ballRigidBody.velocity.y > 0 && ballRigidBody.position.y > initialHeight + 8)
        {
            Vector3 velocity = ballRigidBody.velocity;
            velocity.y *= 0.5f;
            ballRigidBody.velocity = velocity;
        }

    }

    private void fanMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 movement = new Vector3(touch.deltaPosition.x * Time.fixedDeltaTime * touchSensivity * -1, 0, 0);
                if(Physics.Raycast(gunModel.transform.position, Vector3.down, 3f, platformLayer))
                    gunModel.transform.localPosition += movement;
            }
        }
    }
    
    private void blowAir()
    {
        if (Input.touchCount == 0)
            return;

        if(ballRigidBody.position.y > initialHeight + 8 && ballRigidBody.velocity.y > 0)
        {
            Vector3 velocity = ballRigidBody.velocity;
            velocity.y -= 5 * Time.fixedDeltaTime;
            ballRigidBody.velocity = velocity;
            return;
        }

        if (Physics.SphereCast(ballRigidBody.position, 1, Vector3.down, out RaycastHit hit, 2.5f, fanLayer) || ballRigidBody.SweepTest(Vector3.down, out hit, 2.5f))
        {
            if (!hit.transform.CompareTag("Fan"))
                return;

            if(ballRigidBody.velocity.y < 0)
            {
                Vector3 ballVelocity = ballRigidBody.velocity;
                ballVelocity.y *= 0.5f;
                ballRigidBody.velocity = ballVelocity;
            }

            Vector3 force = calculateForce();
            ballRigidBody.AddForceAtPosition(force, gunModel.transform.position);
        }
    }

    private Vector3 calculateForce()
    {
        Vector3 force = Vector3.one;
        Vector3 horizantalForce = Vector3.Normalize(ballRigidBody.position - gunModel.transform.position) * blowPower.x;
        horizantalForce = Vector3.Scale(horizantalForce, gunModel.transform.right * -1);

        Vector3 ballRigidBodyVelocity = Vector3.Scale(ballRigidBody.velocity, gunModel.transform.right);

        if ((ballRigidBodyVelocity.x > 0 && gunModel.transform.localPosition.x > ballRigidBody.transform.localPosition.x) || (ballRigidBodyVelocity.x < 0 && gunModel.transform.localPosition.x < ballRigidBody.transform.localPosition.x))
            horizantalForce *= 2;
        
        Vector3 verticalForce = new Vector3(0, blowPower.y, 0);
        return horizantalForce + verticalForce;
    }


}
