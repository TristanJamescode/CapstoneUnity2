using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy_Spitter : BasicEnemy
{
    protected override void Awake()
    {
        stateMachine = gameObject.AddComponent<StateMachine>();

        BaseState Idle = new IdleState(this, "Idle", stateMachine);
        BaseState Patrol = new PatrolState(this, "Patrol", stateMachine);
        BaseState Chasing = new ChasingState(this, "Chasing", stateMachine);
        BaseState Attacking = new AttackingState(this, "Attacking", stateMachine);

        Transaction idletopatrol = new(Patrol);
        Transaction idletochasing = new(Chasing);
        Transaction patroltochasing = new(Chasing);
        Transaction patroltoidle = new(Idle);
        Transaction chasingtoattacking = new(Attacking);
        Transaction chasingtoidle = new(Idle);
        Transaction attackingtochasing = new(Chasing);
        Transaction attackingtoidle = new(Idle);

        TransactionCondition c_IsPlayerInSight = new C_IsPlayerInRange(this, sightRange);
        TransactionCondition c_IsPlayerInAttack = new C_IsPlayerInRange(this, attackRange);
        TransactionCondition c_IsPlayerInLostRange = new C_IsPlayerInRange(this, sightRange * 1.1f);

        Idle.addTransaction(idletopatrol);
        Idle.addTransaction(idletochasing);
        Patrol.addTransaction(patroltochasing);
        Patrol.addTransaction(patroltoidle);
        Chasing.addTransaction(chasingtoattacking);
        Chasing.addTransaction(chasingtoidle);
        Attacking.addTransaction(attackingtoidle);
        Attacking.addTransaction(attackingtochasing);

        idletochasing.addCondition(c_IsPlayerInSight, true);
        patroltochasing.addCondition(c_IsPlayerInSight, true);
        chasingtoattacking.addCondition(c_IsPlayerInAttack, true);
        chasingtoidle.addCondition(c_IsPlayerInLostRange, false);
        attackingtochasing.addCondition(c_IsPlayerInAttack, false);
        attackingtoidle.addCondition(c_IsPlayerInLostRange, false);

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        stateMachine.setInitState(Idle);
    }
}
