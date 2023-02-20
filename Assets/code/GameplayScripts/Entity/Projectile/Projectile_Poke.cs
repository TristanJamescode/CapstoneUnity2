using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Poke : BasicProjectile
{
    public float PokeDistance = 0;
    public float PokeTime = 1;
    List<Collider> colliders = new List<Collider>();
    public Transform TransformUser;
    protected override void Update()
    {
        //if (collisions < collisions_max) OnDeath();
        maxLifetime -= Time.deltaTime;
        if (maxLifetime < 0) OnDeath();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        collisions++;
        if (other.CompareTag("Player"))
        {
            if (!colliders.Contains(other))
            {
                colliders.Add(other);
                other.GetComponent<BasicEntity>().Take_Damage(Projectile_Damage);
            }
        }
    }
}
