using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateTrigger : MonoBehaviour
{

    public GameObject golfBall;
    public GameObject golfBallPrefab;
    public GameObject extraBalls;

    public int counter;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        extraBalls.transform.position = golfBall.transform.position;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fan"))
            return;

        golfBall.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        /*for(int i = 0; i < counter; i++)
        {

            GameObject ball = Instantiate(golfBallPrefab, Vector3.zero, Quaternion.identity, golfBall.transform);
            ball.transform.localScale = Vector3.one;

            ball.transform.localPosition = Vector3.right * (i + 1) * 0.45f;
        }*/


        GameObject ball = Instantiate(golfBallPrefab, Vector3.zero, Quaternion.identity, golfBall.transform);
        ball.transform.localScale = Vector3.one;
        ball.transform.localPosition = new Vector3(1.15f, 0f, 0f);
        ball.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 270f));
    }

}
