using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 10;

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            health.DecreasedHealth(damageAmount);
        } */

        if(collision.gameObject.TryGetComponent(out BasicPlayer health))
        {
            health.Take_Damage(damageAmount); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out BasicPlayer health))
        {
            health.Take_Damage(damageAmount); 
        }
    }
}
