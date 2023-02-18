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
    public bool playerInSightRange, playerInAttackRange = false;
    public ENEMYSTATE enemystate = ENEMYSTATE.IDLE;
    StateMachine stateMachine;
    public enum ENEMYSTATE{
        IDLE,
        PATROL,
        NOTICE,
        CHASING,
        ATTACKING,
        STAGGER
    }

    protected class IdleState : BaseState
    {
        BasicEnemy enemy;
        public IdleState(BasicEnemy enemy,string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.enemy = enemy;
        }
    }
    protected class PatrolState : BaseState
    {
        BasicEnemy enemy;
        public PatrolState(BasicEnemy enemy, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.enemy = enemy;
        }
    }
    protected class ChasingState : BaseState
    {
        BasicEnemy enemy;
        public ChasingState(BasicEnemy enemy, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.enemy = enemy;
        }
    }
    protected class AttackingState : BaseState
    {
        BasicEnemy enemy;
        public AttackingState(BasicEnemy enemy, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.enemy = enemy;
        }
    }
    protected class IdleToPatrol : Transaction
    {
        BasicEnemy enemy;
        public IdleToPatrol(BasicEnemy enemy, BaseState nextState) : base(nextState)
        {
            this.enemy = enemy;
        }
        public override bool CheckTransaction()
        {
            return true;
        }
    }
    protected class IdleToChasing : Transaction
    {
        BasicEnemy enemy;
        public IdleToChasing(BasicEnemy enemy, BaseState nextState) : base(nextState)
        {
            this.enemy = enemy;
        }
        public override bool CheckTransaction()
        {
            return true;
        }
    }
    protected class PatrolToChasing : Transaction
    {
        BasicEnemy enemy;
        public PatrolToChasing(BasicEnemy enemy, BaseState nextState) : base(nextState)
        {
            this.enemy = enemy;
        }
        public override bool CheckTransaction()
        {
            return true;
        }
    }
    protected class PatrolToIdle : Transaction
    {
        BasicEnemy enemy;
        public PatrolToIdle(BasicEnemy enemy, BaseState nextState) : base(nextState)
        {
            this.enemy = enemy;
        }
        public override bool CheckTransaction()
        {
            return true;
        }
    }
    protected class ChasingToAttacking : Transaction
    {
        BasicEnemy enemy;
        public ChasingToAttacking(BasicEnemy enemy, BaseState nextState) : base(nextState)
        {
            this.enemy = enemy;
        }
        public override bool CheckTransaction()
        {
            return true;
        }
    }
    protected class ChasingToIdle : Transaction
    {
        BasicEnemy enemy;
        public ChasingToIdle(BasicEnemy enemy, BaseState nextState) : base(nextState)
        {
            this.enemy = enemy;
        }
        public override bool CheckTransaction()
        {
            return true;
        }
    }
    protected class AttackingToChasing : Transaction
    {
        BasicEnemy enemy;
        public AttackingToChasing(BasicEnemy enemy, BaseState nextState) : base(nextState)
        {
            this.enemy = enemy;
        }
        public override bool CheckTransaction()
        {
            return true;
        }
    }
    protected class AttackingToIdle : Transaction
    {
        BasicEnemy enemy;
        public AttackingToIdle(BasicEnemy enemy, BaseState nextState) : base(nextState)
        {
            this.enemy = enemy;
        }
        public override bool CheckTransaction()
        {
            return true;
        }
    }
    protected void Awake()
    {
        BaseState Idle = new IdleState(this,"Idle", stateMachine);
        BaseState Patrol = new PatrolState(this,"Patrol", stateMachine);
        BaseState Chasing = new ChasingState(this,"Chasing", stateMachine);
        BaseState Attacking = new AttackingState(this,"Attacking", stateMachine);
        Transaction idletopatrol = new IdleToPatrol(this,Patrol);
        Transaction idletochasing = new IdleToChasing(this, Chasing);
        Transaction patroltochasing = new PatrolToChasing(this, Chasing);
        Transaction patroltoidle = new PatrolToIdle(this, Idle);
        Transaction chasingtoattacking = new ChasingToAttacking(this, Attacking);
        Transaction chasingtoidle = new ChasingToIdle(this, Idle);
        Transaction attackingtochasing = new AttackingToChasing(this, Chasing);
        Transaction attackingtoidle = new AttackingToIdle(this, Idle);

        Idle.addTransaction(idletopatrol);
        Idle.addTransaction(idletochasing);
        Patrol.addTransaction(patroltochasing);
        Patrol.addTransaction(patroltoidle);
        Chasing.addTransaction(chasingtoattacking);
        Chasing.addTransaction(chasingtoidle);
        Attacking.addTransaction(attackingtoidle);
        Attacking.addTransaction(attackingtochasing);

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        stateMachine.Update();
        TransactionCheck();
        RunState();
    }

    protected virtual void RunState()
    {
        switch (enemystate)
        {
            case ENEMYSTATE.IDLE:
                break;
            case ENEMYSTATE.PATROL:
                if (!walkPointSet) { SearchRandomWalkPoint(); }
                else { agent.SetDestination(walkPoint); }
                break;
            case ENEMYSTATE.CHASING:
                agent.SetDestination(Player.position);
                transform.LookAt(Player);
                break;
            case ENEMYSTATE.ATTACKING:
                agent.SetDestination(transform.position);
                transform.LookAt(Player);
                if (!alreadyAttacked)
                {
                    alreadyAttacked = true;
                }
                break;
            default:
                break;
        }
    }
    protected virtual void SearchRandomWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z+randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    protected virtual void TransactionCheck()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        switch (enemystate)
        {
            case ENEMYSTATE.IDLE:
                if (!playerInSightRange&&Random.Range(0,10)==0) enemystate = ENEMYSTATE.PATROL; //IDLE TO PATROL
                if (playerInSightRange || playerInAttackRange) enemystate = ENEMYSTATE.CHASING; //IDLE TO CHASING
                break;
            case ENEMYSTATE.PATROL:
                if (playerInSightRange && !playerInAttackRange) enemystate = ENEMYSTATE.CHASING; //PATROL TO CHASING
                if ((transform.position - walkPoint).magnitude < 1f) //PATROL TO IDLE 
                {
                    walkPointSet = false;//Walkpoint reached
                    enemystate = ENEMYSTATE.IDLE;
                }
                break;
            case ENEMYSTATE.CHASING:
                if (!playerInSightRange) enemystate = ENEMYSTATE.IDLE; //CHASING TO IDLE
                if (playerInAttackRange) enemystate = ENEMYSTATE.ATTACKING; //CHASING TO ATTACK

                break;
            case ENEMYSTATE.ATTACKING:
                if (!playerInAttackRange&&alreadyAttacked) enemystate = ENEMYSTATE.CHASING; //ATTACKING TO CHASING
                break;
            default:
                break;
        }
    }
}
