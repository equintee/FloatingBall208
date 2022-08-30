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


    public async void endGame(bool playerWin)
    {
        disableScripts();
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


    private void disableScripts()
    {
        foreach (MonoBehaviour script in FindObjectsOfType<MonoBehaviour>())
            script.enabled = false;
    }

}
