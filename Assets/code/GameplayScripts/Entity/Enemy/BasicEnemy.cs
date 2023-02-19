using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Reference https://www.youtube.com/watch?v=UjkSFoLxesw
/// </summary>
public class BasicEnemy : BasicEntity
{
    public NavMeshAgent agent;
    public Transform Player;
    public LayerMask whatIsGround, whatIsPlayer;
    //Patrol
    public Vector3 walkPoint;
    bool walkPointSet = false;
    public float walkPointRange;
    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //States
    public float sightRange, attackRange;
    protected StateMachine stateMachine;
    public string Statename;
    protected class IdleState : BaseState
    {
        BasicEnemy enemy;
        public IdleState(BasicEnemy enemy,string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.enemy = enemy;
        }
        public override void Update()
        {
            enemy.agent.SetDestination(enemy.transform.position);
        }
    }
    protected class PatrolState : BaseState
    {
        int giveupcount=0;
        BasicEnemy enemy;
        public PatrolState(BasicEnemy enemy, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.enemy = enemy;
        }
        public override void OnEnter()
        {
            enemy.walkPointSet = false;
            for (int i= 0; i < giveupcount; i++){
                if (enemy.SearchRandomWalkPoint())
                {
                    enemy.walkPointSet = true;
                    return;
                }
            }
        }
        public override void Update()
        {
            if(enemy.walkPointSet)enemy.agent.SetDestination(enemy.walkPoint);
        }
    }
    protected class ChasingState : BaseState
    {
        BasicEnemy enemy;
        public ChasingState(BasicEnemy enemy, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.enemy = enemy;
        }
        public override void Update()
        {
                enemy.agent.SetDestination(enemy.Player.position);
                enemy.transform.LookAt(enemy.Player);
        }
    }
    protected class AttackingState : BaseState
    {
        BasicEnemy enemy;
        public AttackingState(BasicEnemy enemy, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.enemy = enemy;
        }
        public override void Update()
        {
            enemy.agent.SetDestination(enemy.transform.position);
            enemy.transform.LookAt(enemy.Player);
            if (!enemy.alreadyAttacked)
            {
                enemy.alreadyAttacked = true;
            }
        }
    }
    protected class C_IsPlayerInRange : TransactionCondition
    {
        float Range=0;
        BasicEnemy enemy;
        public C_IsPlayerInRange(BasicEnemy enemy, float Range)
        {
            this.Range = Range;
            this.enemy = enemy;
        }
        public override bool TriggerCheck()
        {
            return Physics.CheckSphere(enemy.transform.position, Range, enemy.whatIsPlayer);
        }
    }
    protected class C_IsAttackFinished : TransactionCondition
    {
        BasicEnemy enemy;
        public C_IsAttackFinished(BasicEnemy enemy)
        {
            this.enemy = enemy;
        }
        public override bool TriggerCheck()
        {
            return (enemy.alreadyAttacked);
        }
    }
    protected class C_IsReachedtoWalkPoint: TransactionCondition
    {
        BasicEnemy enemy;
        public C_IsReachedtoWalkPoint(BasicEnemy enemy)
        {
            this.enemy = enemy;
        }
        public override bool TriggerCheck()
        {
            bool returnbool=false;
            if ((enemy.transform.position - enemy.walkPoint).magnitude < 1f) //PATROL TO IDLE 
            {
                enemy.walkPointSet = false;//Walkpoint reached
                returnbool = true;
            }
            return returnbool;
        }
    }
    protected virtual void Awake()
    {
        stateMachine = gameObject.AddComponent<StateMachine>();

        BaseState Idle = new IdleState(this,"Idle", stateMachine);
        BaseState Patrol = new PatrolState(this,"Patrol", stateMachine);
        BaseState Chasing = new ChasingState(this,"Chasing", stateMachine);
        BaseState Attacking = new AttackingState(this,"Attacking", stateMachine);

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
        TransactionCondition c_IsPlayerInLostRange = new C_IsPlayerInRange(this, sightRange*1.1f);

        Idle.addTransaction(idletopatrol);
        Idle.addTransaction(idletochasing);
        Patrol.addTransaction(patroltochasing);
        Patrol.addTransaction(patroltoidle);
        Chasing.addTransaction(chasingtoattacking);
        Chasing.addTransaction(chasingtoidle);
        Attacking.addTransaction(attackingtoidle);
        Attacking.addTransaction(attackingtochasing);

        idletochasing.addCondition(c_IsPlayerInSight,true);
        patroltochasing.addCondition(c_IsPlayerInSight,true);
        chasingtoattacking.addCondition(c_IsPlayerInAttack,true);
        chasingtoidle.addCondition(c_IsPlayerInLostRange, false);
        attackingtochasing.addCondition(c_IsPlayerInAttack, false);
        attackingtoidle.addCondition(c_IsPlayerInLostRange, false);

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        stateMachine.setInitState(Idle);
    }

    protected override void Update()
    {
        stateMachine.Update();
        Statename = stateMachine.currentState.name;
    }
    protected virtual bool SearchRandomWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z+randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            return true;
        }
        return false;
    }
}
