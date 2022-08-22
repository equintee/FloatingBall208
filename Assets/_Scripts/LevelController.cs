using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelController : MonoBehaviour
{
    public float touchSensivity;
    public float movementSpeed;
    public Vector2 blowPower;


    public async void endGame()
    {
        disableScripts();
        //Cinemachine animator

        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        GameObject hoop = GameObject.FindGameObjectWithTag("Hoop");

        ball.GetComponent<Rigidbody>().isKinematic = true;

        ball.transform.DOMoveZ(hoop.transform.position.z, 1f).SetEase(Ease.Linear);
        await ball.transform.DOMoveY(hoop.transform.position.y + 0.5f, 1f).SetEase(Ease.InQuint).AsyncWaitForCompletion();
        Debug.Log("ASD");


    }


    private void disableScripts()
    {
        foreach (MonoBehaviour script in FindObjectsOfType<MonoBehaviour>())
            script.enabled = false;
    }
}
