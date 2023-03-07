using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Reference https://www.youtube.com/watch?v=UjkSFoLxesw
/// </summary>
public class BasicEnemy : BasicEntity
{
    //[SerializeField] private Rigidbody body;
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
    protected StateMachine _StateMachine;
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
    protected class StunState : BaseState
    {
        BasicEnemy enemy;
        public StunState(BasicEnemy enemy, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.enemy = enemy;
        }
        public override void OnEnter()
        {
        }
        public override void Update()
        {
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
    protected class C_IsStun : TransactionCondition
    {
        BasicEnemy enemy;
        public C_IsStun(BasicEnemy enemy)
        {
            this.enemy = enemy;
        }
        public override bool TriggerCheck()
        {
            return enemy.Knockback_Counter <= 0;
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
    protected BaseState State_Idle, State_Patrol, State_Chasing, State_Attacking, State_Stun;
    protected virtual void Awake()
    {
        if (Player == null && GameObject.FindGameObjectWithTag("Player") != null) { Player = GameObject.FindGameObjectWithTag("Player").transform; } //Find Player if exists
        _StateMachine = gameObject.AddComponent<StateMachine>();

        State_Idle = new IdleState(this, "Idle", _StateMachine);
        State_Patrol = new PatrolState(this, "Patrol", _StateMachine);
        State_Chasing = new ChasingState(this, "Chasing", _StateMachine);
        State_Attacking = new AttackingState(this, "Attacking", _StateMachine);
        State_Stun = new StunState(this, "Stun", _StateMachine);

        Transaction idletopatrol = new(State_Patrol);
        Transaction idletochasing = new(State_Chasing);
        Transaction patroltochasing = new(State_Chasing);
        Transaction patroltoidle = new(State_Idle);
        Transaction chasingtoattacking = new(State_Attacking);
        Transaction chasingtoidle = new(State_Idle);
        Transaction chasingtoidle2 = new(State_Idle);
        Transaction attackingtochasing = new(State_Chasing);
        Transaction attackingtoidle = new(State_Idle);
        Transaction stuntoidle = new(State_Idle);

        TransactionCondition c_IsPlayerInAttackRange = new C_IsPlayerInRange(this, attackRange);
        TransactionCondition c_IsPlayerInAttackOutRange = new C_IsPlayerInRange(this, attackRange * 1.1f);
        TransactionCondition c_IsPlayerInSightRange = new C_IsPlayerInRange(this, sightRange);
        TransactionCondition c_IsPlayerInLostRange = new C_IsPlayerInRange(this, lostRange);
        //TransactionCondition c_IsAttackFinished = new C_IsAttackFinished(this);
        TransactionCondition c_IsTimetoLost = new C_IsTimetoLost(this);
        TransactionCondition c_IsStun = new C_IsStun(this);

        State_Idle.addTransaction(idletopatrol);
        State_Idle.addTransaction(idletochasing);
        State_Patrol.addTransaction(patroltochasing);
        State_Patrol.addTransaction(patroltoidle);
        State_Chasing.addTransaction(chasingtoattacking);
        State_Chasing.addTransaction(chasingtoidle);
        State_Chasing.addTransaction(chasingtoidle2);
        State_Attacking.addTransaction(attackingtoidle);
        State_Attacking.addTransaction(attackingtochasing);
        State_Stun.addTransaction(stuntoidle);

        idletochasing.addCondition(c_IsPlayerInSightRange, true);
        patroltochasing.addCondition(c_IsPlayerInSightRange, true);
        chasingtoattacking.addCondition(c_IsPlayerInAttackRange, true);
        //chasingtoattacking.addCondition(c_IsPlayerInSightRange, false);
        chasingtoidle.addCondition(c_IsPlayerInLostRange, false);
        chasingtoidle2.addCondition(c_IsTimetoLost, true);
        //attackingtochasing.addCondition(c_IsAttackFinished, true);
        attackingtochasing.addCondition(c_IsPlayerInAttackOutRange, false);
        attackingtoidle.addCondition(c_IsPlayerInLostRange, false);
        stuntoidle.addCondition(c_IsStun, true);

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        _StateMachine.setInitState(State_Idle);
    }
    protected override void Update()
    {
        base.Update();
        _StateMachine.Update();
        Statename = _StateMachine.currentState.name;
    }
    public override void Take_Knockback(float Amount, Vector3 Direction)
    {
        base.Take_Knockback(Amount, Direction);
        Knockback_Counter = 2;
        _StateMachine.ChangeState(State_Stun);
        Direction.Normalize();
        Vector3 KnockbackVector = Direction * Amount;
        Knockback_Velocity = KnockbackVector;
    }
    public override void Update_KnockbackRelated()
    {
        base.Update_KnockbackRelated();
        if (Knockback_Velocity != Vector3.zero)
        {
            agent.velocity = Knockback_Velocity;
            Knockback_Velocity *= 0.9f;
            if (Knockback_Velocity.magnitude < 0.5f) Knockback_Velocity = Vector3.zero;
        }
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
