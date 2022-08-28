using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;

public class gunModelController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Trap"))
            return;
        GetComponent<BoxCollider>().enabled = false;
        FindObjectOfType<LevelController>().endGame(false);
    }
}
