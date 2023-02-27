using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FallenCard : BasicEnemy
{
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
        //agent.SetDestination(transform.position); //Make Enemy does not move
        transform.LookAt(Player);
        if (attack_ready)
        {
            timeToAttack = timeBetweenAttacks;
            Vector3 direction = transform.rotation * Vector3.forward;
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
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.body.CompareTag("Player"))
        {
            collision.body.GetComponent<BasicEntity>().Take_Damage(10);
        }
    }
}
