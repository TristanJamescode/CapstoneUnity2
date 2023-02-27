using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighting : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Gameobject tag is: " + other.gameObject.tag); 
    //    if(other.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log("Enemy detected"); 
    //        Destroy(other.gameObject);
    //    }    
    //}
}
