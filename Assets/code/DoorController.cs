using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Source
//https://www.youtube.com/watch?v=FXIJFVwxUbU

public class DoorController : MonoBehaviour
{
    public GameObject Door;
    public bool doorIsOpening;

    // Update is called once per frame
    void Update()
    {
        if(doorIsOpening == true) { 
        Door.transform.Translate (Vector3.up * Time.deltaTime * 5);
        }
        if (Door.transform.position.y > 257)
        {
            doorIsOpening = false;  
        }
        //thing
        if (Door.transform.position.y == 257)
        {
            doorIsOpening = false;
        }
    }

    private void OnMouseDown()
    {
        doorIsOpening= true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
      {
            doorIsOpening= true;
       }

    }

}
