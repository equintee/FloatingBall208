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

        await ball.transform.DOMoveZ(ball.transform.position.z + 1f, 1f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        Debug.Log("ASD");


    }


    private void disableScripts()
    {
        foreach (MonoBehaviour script in FindObjectsOfType<MonoBehaviour>())
            script.enabled = false;
    }
}
