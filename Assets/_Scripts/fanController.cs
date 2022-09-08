using DG.Tweening;
using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class fanController : MonoBehaviour
{
    public GameObject ball;
    public GameObject gunModel;
    public GameObject particle;
    public float maximumHeight;
    [HideInInspector] public bool win = false;

    private Rigidbody ballRigidBody;
    private float touchSensivity;
    private Vector3 blowPower;
    private float movementSpeed;
    private int platformLayer;
    private int fanLayer;
    private PathCreator pathCreator;
    private float distanceTravelled = 0f;
    private Vector3 velocity;
    private void Awake()
    {
        LevelController levelController = FindObjectOfType<LevelController>();

        ballRigidBody = ball.GetComponent<Rigidbody>();


        touchSensivity = levelController.touchSensivity;
        blowPower = levelController.blowPower;
        movementSpeed = levelController.movementSpeed;
        platformLayer = LayerMask.GetMask("Platform");
        fanLayer = LayerMask.GetMask("Fan");
        pathCreator = FindObjectOfType<PathCreator>();
        velocity = Vector3.zero;

        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
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
            FindObjectOfType<LevelController>().endGame(win);
            return;
        }

        if(ballRigidBody.transform.position.y < getMaxHeight() - 4f)
        {
            FindObjectOfType<LevelController>().endGame(false);
            return;
        }
            
    }
    
    private void moveFanAndPlayer()
    {
        distanceTravelled += movementSpeed * Time.fixedDeltaTime;
        Vector3 destination = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);


        velocity = transform.InverseTransformDirection(ballRigidBody.velocity);
        velocity.z = 0;
        ballRigidBody.velocity = transform.TransformDirection(velocity);
        
        if (ballRigidBody.position.y > getMaxHeight() && ballRigidBody.velocity.y > 0)
        {
            Vector3 velocity = ballRigidBody.velocity;
            velocity.y = 0;
            ballRigidBody.velocity = velocity;
        }

        transform.position = destination;
        Quaternion rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
        transform.rotation = rotation; 
    }

    private void fanMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 movement = new Vector3(touch.deltaPosition.x * touchSensivity * -1, 0, 0);
                if(Physics.Raycast(gunModel.transform.position + movement, Vector3.down, 100f, platformLayer))
                    gunModel.transform.localPosition += movement;
            }
        }
    }
    
    private void blowAir()
    {
        if (Input.touchCount == 0)
        {
            DOTween.Kill(particle.transform);
            particle.transform.GetComponent<ParticleSystem>().Stop();
            particle.transform.GetComponent<ParticleSystem>().Clear();
            ballRigidBody.useGravity = true;
            return;
        }

        if(particle.GetComponent<ParticleSystem>().isPlaying == false)
        {
            DOTween.Kill(particle.transform);
            particle.transform.localScale = new Vector3(5, 5, 2);
            particle.transform.DOScale(new Vector3(5,5,8), 0.5f).SetEase(Ease.OutQuart);
            particle.transform.GetComponent<ParticleSystem>().Play();
        }

        if (Physics.SphereCast(ballRigidBody.position, 1, Vector3.down, out RaycastHit hit, getMaxHeight(), fanLayer) || ballRigidBody.SweepTest(Vector3.down, out hit, 2.5f))
        {
            if (!hit.transform.CompareTag("Fan"))
                return;
            Vector3 force = calculateForce();
            ballRigidBody.velocity += transform.TransformDirection(force * Time.fixedDeltaTime);
            if(ballRigidBody.position.y >= getMaxHeight())
            {
                Vector3 velocity = transform.InverseTransformDirection(ballRigidBody.velocity);
                velocity.y = 0;
                ballRigidBody.velocity = transform.TransformDirection(velocity);
                ballRigidBody.useGravity = false;
            }
        }
        else
            ballRigidBody.useGravity = true;
    }

    private Vector3 calculateForce()
    {
        
        Vector3 horizantalForce = Vector3.Normalize(ballRigidBody.position - gunModel.transform.position) * blowPower.x;

        Vector3 ballRigidBodyVelocity = transform.InverseTransformDirection(ballRigidBody.velocity);

        if ((ballRigidBodyVelocity.x > 0 && gunModel.transform.localPosition.x > ballRigidBody.transform.localPosition.x) || (ballRigidBodyVelocity.x < 0 && gunModel.transform.localPosition.x < ballRigidBody.transform.localPosition.x))
            horizantalForce *= 2;

        //horizantalForce = Vector3.Scale(gunModel.transform.right, horizantalForce);

        Vector3 verticalForce = gunModel.transform.position.y > getMaxHeight() ? Vector3.zero : new Vector3(0, blowPower.y, 0);

        verticalForce.y = ballRigidBody.velocity.y < 0 ? verticalForce.y * 2f : verticalForce.y;
        verticalForce.y = ballRigidBody.position.y > getMaxHeight() ? 0f : verticalForce.y;

        Vector3 velocity = transform.InverseTransformDirection(horizantalForce + verticalForce);
        velocity.z = 0;
        return velocity;
    }

    private float getMaxHeight()
    {
        return gunModel.transform.position.y + maximumHeight;
    }

}
