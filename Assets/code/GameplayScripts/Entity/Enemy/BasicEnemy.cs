using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This inheretated from BasicEntity, Health
/// </summary>
public class BasicEnemy : BasicEntity
{
    protected int AttackDamage = 2;
    protected int AttackDelay = 5; //This delay attack 5 frames
    protected override void Update()
    {
        if (AttackDelay > 0) AttackDelay--;
        if (AttackDelay == 0) Attack();
    }
    protected virtual void Attack()
    {

    }
}
