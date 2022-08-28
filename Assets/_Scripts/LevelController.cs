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
            GameObject ball = GameObject.FindGameObjectWithTag("Ball");
            await ball.transform.DOMoveZ(ball.transform.position.z + 1f, 1f).SetEase(Ease.Linear).AsyncWaitForCompletion();
            Debug.Log("ASD");
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
