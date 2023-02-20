using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Contactor : BasicEnemy
{
    public float PokeDistance = 0.3f;
    public Projectile_Poke Projectile;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override bool AttackPlayer()
    {
        agent.SetDestination(transform.position); //Make Enemy does not move
        Vector3 lookAtPosition = new Vector3(Player.position.x, transform.position.y, Player.position.z);
        transform.LookAt(lookAtPosition);
        if (attack_ready)
        {
            timeToAttack = timeBetweenAttacks;
            Vector3 direction = transform.rotation * Vector3.forward;
            Projectile_Poke currentBullet = Instantiate(Projectile, transform.position, Quaternion.identity);
            currentBullet.transform.rotation = transform.rotation;
            currentBullet.PokeDistance = PokeDistance;
            currentBullet.maxLifetime = 0.8f;
            currentBullet.TransformUser = this.transform;
            currentBullet.GetComponent<Rigidbody>().AddForce(direction * 1.2f, ForceMode.Impulse);
            attack_ready = false;
            return true;
        }
        else
        {
            timeToAttack -= Time.deltaTime;
            if (timeToAttack < 0) attack_ready = true;
            return false;
        }
    }
}
