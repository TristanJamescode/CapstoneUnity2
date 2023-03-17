using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollision : MonoBehaviour
{
    private float damage=0;
    private Vector3 KnockbackDirection;
    private float knockbackAmount;
    public void ChangeDamage(float damage)
    {
        this.damage = damage;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.tag);
        if (collision.collider.CompareTag("Enemy"))
        { 
            collision.collider.GetComponentInParent<BasicEntity>().Take_Damage(damage);
            //collision.collider.GetComponentInParent<BasicEnemy>().Take_Knockback(knockbackAmount, KnockbackDirection);
        }
    }
}
