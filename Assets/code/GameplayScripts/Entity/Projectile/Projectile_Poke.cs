using System.Collections.Generic;
using UnityEngine;

public class Projectile_Poke : BasicProjectile
{
    public float PokeDistance = 0;
    private float PokeDistances = 0;
    private int PokeState=0;
    public float PokeTime = 1;
    List<Collider> colliders = new List<Collider>();
    public Transform TransformUser;
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        if (TransformUser == null)
        {
            OnDeath();
            return;
        }
        transform.position = TransformUser.position+transform.rotation*Vector3.forward*PokeDistances;
        switch (PokeState)
        {
            case 0://Poke
                PokeDistances+=PokeDistance*Time.deltaTime/PokeTime;
                if (PokeDistances > PokeDistance) PokeState = 1;
                break;
            case 1://Stay
                PokeState = 2;
                break;
            case 2://Pull
                PokeDistances -= PokeDistance * Time.deltaTime / PokeTime;
                if (PokeDistances < 0) OnDeath();
                break;
            default:
                break;
        }
        maxLifetime -= Time.deltaTime;
        if (maxLifetime < 0) OnDeath();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!colliders.Contains(other))
            {
                colliders.Add(other);
                other.GetComponent<BasicEntity>().Take_Damage(Projectile_Damage);
            }
        }
    }
    protected override void OnCollisionEnter(Collision collision)
    {
    }
}
