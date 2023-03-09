using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPowerup : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.CompareTag("Battery"))
        {
            Destroy(c.gameObject); 

        }
    }
}
