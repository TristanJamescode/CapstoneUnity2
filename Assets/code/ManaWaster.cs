using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//piece of test code to test out mana function
public class ManaWaster : MonoBehaviour
{
    public float Manawaste = 10;

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            health.DecreasedHealth(damageAmount);
        } */

        if (collision.gameObject.TryGetComponent(out PlayerMana mana))
        {
            mana.DecreasedMana(Manawaste);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMana mana))
        {
            mana.DecreasedMana(Manawaste);
        }
    }
}
