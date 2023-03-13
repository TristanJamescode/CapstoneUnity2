using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFire : MonoBehaviour
{
    private void OnTriggerEnter(Collider GObj)
    {
        Debug.Log("Collided with:" + GObj); 
        if(GObj.gameObject.tag == "CanCatchFire")
        {
            //This is where we set an object on fire. 
            GObj.GetComponent<FireManager>().Fire.SetActive(true); 
        }
    }
}
