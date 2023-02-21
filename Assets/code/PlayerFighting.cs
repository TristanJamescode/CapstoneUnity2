using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighting : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Destroy(other.gameObject); 
        }
    }
}
