using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System.Threading.Tasks;

public class LevelController : MonoBehaviour
{
    public float touchSensivity;
    public float movementSpeed;
    public Vector2 blowPower;

    private void Awake()
    {
        changeScriptBehaviour(false);
        FindObjectOfType<fanController>().ball.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void Update()
    {
        if(Input.touchCount > 0)
        {
            changeScriptBehaviour(true);
            FindObjectOfType<fanController>().ball.GetComponent<Rigidbody>().isKinematic = false;
            this.enabled = false;
        }
    }
    public async void endGame(bool playerWin)
    {
        changeScriptBehaviour(false);
        FindObjectOfType<fanController>().particle.GetComponent<ParticleSystem>().Stop();
        FindObjectOfType<fanController>().particle.GetComponent<ParticleSystem>().Clear();
        //Cinemachine animator
        if (playerWin)
        {
            Transform ballTransform = FindObjectOfType<fanController>().ball.transform;
            Transform hole = GameObject.Find("Hole1").transform;

            ballTransform.DOMoveX(hole.position.x, 1f).SetEase(Ease.Linear);
            ballTransform.DOMoveZ(hole.position.z, 1f).SetEase(Ease.Linear);
            await ballTransform.DOMoveY(hole.position.y, 1f).SetEase(Ease.InQuad).AsyncWaitForCompletion();
            
        }
        else
        {
            GameObject fan = GameObject.FindGameObjectWithTag("Fan");

            fan.GetComponent<Rigidbody>().isKinematic = false;

            FindObjectOfType<CinemachineStateDrivenCamera>().LiveChild.Follow = null;
            FindObjectOfType<CinemachineStateDrivenCamera>().LiveChild.LookAt = null;

            await Task.Delay(System.TimeSpan.FromSeconds(1f));
            Debug.Log("lose");
        }

    }


    private void changeScriptBehaviour(bool value)
    {
        foreach (MonoBehaviour script in FindObjectsOfType<fanController>())
            script.enabled = value;
    }

}
