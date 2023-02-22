using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    [SerializeField] GameObject GameOverScreen;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameOverScreen.SetActive(true); 
        }
        Destroy(other.gameObject); 
    }
}
