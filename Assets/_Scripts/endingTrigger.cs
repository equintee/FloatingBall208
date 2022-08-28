using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<BoxCollider>().enabled = false;
        FindObjectOfType<LevelController>().endGame(true);
        return;
    }
}
