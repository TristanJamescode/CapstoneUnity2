using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelOneStart : MonoBehaviour
{
    GameObject Player; 
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player"); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && SceneManager.GetActiveScene().name != "LevelThree")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if(SceneManager.GetActiveScene().name == "LevelThree" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("MainArea"); 
        }
    }
}
