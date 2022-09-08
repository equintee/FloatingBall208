using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using DG.Tweening;

[System.Serializable]
public struct canvasList
{
    public GameObject tapToStart;
    public GameObject levelCompleteUI;
    public GameObject super;
    public GameObject failUI;
}
public class LevelController : MonoBehaviour
{
    public float touchSensivity;
    public float movementSpeed;
    public Vector2 blowPower;
    public canvasList canvasList;

    private void Awake()
    {
        changeScriptBehaviour(false);
        FindObjectOfType<fanController>().ball.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void Update()
    {
        if(Input.touchCount > 0)
        {
            canvasList.tapToStart.SetActive(false);
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
            GameObject superImage = Instantiate(canvasList.super, hole.position + new Vector3(0,1,0), FindObjectOfType<fanController>().transform.rotation, null);
            superImage.transform.DOMoveY(superImage.transform.position.y + 2f, 1f).SetEase(Ease.OutSine).OnComplete(() => Destroy(superImage));

            await Task.Delay(System.TimeSpan.FromSeconds(0.5f));
            canvasList.levelCompleteUI.SetActive(true);
        }
        else
        {
            GameObject fan = GameObject.FindGameObjectWithTag("Fan");

            fan.GetComponent<Rigidbody>().isKinematic = false;
            FindObjectOfType<fanController>().ball.GetComponent<Rigidbody>().useGravity = true;
            FindObjectOfType<CinemachineStateDrivenCamera>().LiveChild.Follow = null;
            FindObjectOfType<CinemachineStateDrivenCamera>().LiveChild.LookAt = null;

            await Task.Delay(System.TimeSpan.FromSeconds(1.5f));
            canvasList.failUI.SetActive(true);
        }

    }


    private void changeScriptBehaviour(bool value)
    {
        foreach (MonoBehaviour script in FindObjectsOfType<fanController>())
            script.enabled = value;
    }

    public void changeScene()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        level++;
        level %= SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(level);
    }
}
