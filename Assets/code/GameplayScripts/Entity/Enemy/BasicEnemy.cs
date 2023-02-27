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
    protected float timeToAttack = 0.0f;
    public float timeBetweenAttacks = 3.0f;
    protected bool attack_ready;
    protected bool attack_finished;
    //States
    public float attackRange, sightRange, lostRange;
    protected float TimetoLost = 0.0f;
    public float TimetoLostEntity = 3.0f;
    protected StateMachine stateMachine;
    public string Statename;
    protected class IdleState : BaseState
    {
        BasicEnemy enemy;
        public IdleState(BasicEnemy enemy, string name, StateMachine stateMachine) : base(name, stateMachine)
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
        int giveupcount = 0;
        BasicEnemy enemy;
        public PatrolState(BasicEnemy enemy, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.enemy = enemy;
        }
        public override void OnEnter()
        {
            enemy.walkPointSet = false;
            for (int i = 0; i < giveupcount; i++)
            {
                if (enemy.SearchRandomWalkPoint())
                {
                    enemy.walkPointSet = true;
                    return;
                }
            }
        }
        public override void Update()
        {
            if (enemy.walkPointSet) enemy.agent.SetDestination(enemy.walkPoint);
        }
    }
    protected class ChasingState : BaseState
    {
        BasicEnemy enemy;
        public ChasingState(BasicEnemy enemy, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.enemy = enemy;
        }
        public override void OnEnter()
        {
            enemy.TimetoLost = enemy.TimetoLostEntity;
        }
        public override void Update()
        {
            if (enemy.IsPlayerBetweenSightandLost())
            {
                enemy.TimetoLost -= Time.deltaTime;
            }
            else
            {
                enemy.TimetoLost = enemy.TimetoLostEntity;
            }
            enemy.agent.SetDestination(enemy.Player.position);
            Vector3 lookatpos = new Vector3(enemy.Player.position.x, enemy.transform.position.y, enemy.Player.position.z);
            enemy.transform.LookAt(lookatpos);
        }
    }
    protected class AttackingState : BaseState
    {
        BasicEnemy enemy;
        public AttackingState(BasicEnemy enemy, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.enemy = enemy;
        }
        public override void OnEnter()
        {
            enemy.attack_finished = false;
            enemy.attack_ready = false;
        }
        public override void Update()
        {
            enemy.AttackPlayer();
        }
    }
    protected class C_IsPlayerInRange : TransactionCondition
    {
        protected float Range = 0;
        protected BasicEnemy enemy;
        public C_IsPlayerInRange(BasicEnemy enemy, float Range)
        {
            this.Range = Range;
            this.enemy = enemy;
        }
        public override bool TriggerCheck()
        {
            bool returnbool = Physics.CheckSphere(enemy.transform.position, Range, enemy.whatIsPlayer);
            //if (returnbool) Debug.Log(Range);
            return returnbool;
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
            return (enemy.attack_finished);
        }
    }
    protected class C_IsTimetoLost : TransactionCondition
    {
        BasicEnemy enemy;
        public C_IsTimetoLost(BasicEnemy enemy)
        {
            this.enemy = enemy;
        }
        public override bool TriggerCheck()
        {
            return (enemy.TimetoLost < 0);
        }
    }
    protected class C_IsReachedtoWalkPoint : TransactionCondition
    {
        BasicEnemy enemy;
        public C_IsReachedtoWalkPoint(BasicEnemy enemy)
        {
            this.enemy = enemy;
        }
        public override bool TriggerCheck()
        {
            bool returnbool = false;
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
        if (Player == null && GameObject.FindGameObjectWithTag("Player") != null) { Player = GameObject.FindGameObjectWithTag("Player").transform; } //Find Player if exists
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
        Transaction chasingtoidle2 = new(Idle);
        Transaction attackingtochasing = new(Chasing);
        Transaction attackingtoidle = new(Idle);

        TransactionCondition c_IsPlayerInAttackRange = new C_IsPlayerInRange(this, attackRange);
        TransactionCondition c_IsPlayerInAttackOutRange = new C_IsPlayerInRange(this, attackRange * 1.1f);
        TransactionCondition c_IsPlayerInSightRange = new C_IsPlayerInRange(this, sightRange);
        TransactionCondition c_IsPlayerInLostRange = new C_IsPlayerInRange(this, lostRange);
        //TransactionCondition c_IsAttackFinished = new C_IsAttackFinished(this);
        TransactionCondition c_IsTimetoLost = new C_IsTimetoLost(this);

        Idle.addTransaction(idletopatrol);
        Idle.addTransaction(idletochasing);
        Patrol.addTransaction(patroltochasing);
        Patrol.addTransaction(patroltoidle);
        Chasing.addTransaction(chasingtoattacking);
        Chasing.addTransaction(chasingtoidle);
        Chasing.addTransaction(chasingtoidle2);
        Attacking.addTransaction(attackingtoidle);
        Attacking.addTransaction(attackingtochasing);

        idletochasing.addCondition(c_IsPlayerInSightRange, true);
        patroltochasing.addCondition(c_IsPlayerInSightRange, true);
        chasingtoattacking.addCondition(c_IsPlayerInAttackRange, true);
        //chasingtoattacking.addCondition(c_IsPlayerInSightRange, false);
        chasingtoidle.addCondition(c_IsPlayerInLostRange, false);
        chasingtoidle2.addCondition(c_IsTimetoLost, true);
        //attackingtochasing.addCondition(c_IsAttackFinished, true);
        attackingtochasing.addCondition(c_IsPlayerInAttackOutRange, false);
        attackingtoidle.addCondition(c_IsPlayerInLostRange, false);

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        stateMachine.setInitState(Idle);
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.Update();
        Statename = stateMachine.currentState.name;
    }
    protected virtual bool SearchRandomWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            return true;
        }
        return false;
    }
    protected virtual bool AttackPlayer()
    {
        return false;
    }
    protected virtual bool IsPlayerBetweenSightandLost()
    {
        return (Physics.CheckSphere(transform.position, sightRange, whatIsPlayer)
            && !Physics.CheckSphere(transform.position, lostRange, whatIsPlayer));

    }
}
