using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFire : MonoBehaviour
{
    private void OnTriggerEnter(Collider GObj)
    {
        if(GObj.gameObject.tag == "CanCatchFire")
        {
            //This is where we set an object on fire. 
            GObj.GetComponent<FireManager>().Fire.SetActive(true); 
        }
        else if (GObj.gameObject.tag == "Enemy")
        {
            if (GObj.GetComponent<BasicEnemy>() != null)
            {
                GObj.GetComponent<BasicEnemy>().ChangeState_OnFire();
            } else if (GObj.GetComponentInParent<BasicEnemy>() != null)
            {
                GObj.GetComponentInParent<BasicEnemy>().ChangeState_OnFire();
            }
        }
    }
}
