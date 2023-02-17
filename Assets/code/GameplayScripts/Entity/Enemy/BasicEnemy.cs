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
    public bool playerInSightRange, playerInAttackRange;
    public ENEMYSTATE enemystate = ENEMYSTATE.IDLE;
    public enum ENEMYSTATE{
        IDLE,
        PATROL,
        NOTICE,
        CHASING,
        ATTACKING,
        STAGGER
    }

    protected void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
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
