using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbTop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Player Is At The Top"); 
            other.gameObject.GetComponent<PlayerControl>().PlayerClimbTop = true;
            other.gameObject.GetComponent<PlayerControl>().ClimbTarget = this.gameObject.GetComponentInChildren<Transform>(); 
        }
    }
}
