using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;

public class gunModelController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trap"))
        {
            GetComponent<BoxCollider>().enabled = false;
            FindObjectOfType<LevelController>().endGame(false);
        }
        else if (other.CompareTag("Goal"))
        {
            FindObjectOfType<fanController>().win = true;
            other.GetComponent<SpriteRenderer>().color = new Color32(0, 139, 255, 255);
        }
            
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.collider.enabled = false;
        FindObjectOfType<LevelController>().endGame(false);
    }
}
