using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : EnemySlime
{
 
    public Transform PlayerTargetGhost;
    [SerializeField] float timerGhost;
    private float moveTimeGhost = 5;
    private float beginTimer;
    [SerializeField] bool isGhostMoving;
    private bool didPlayerCollide; 
    private float waitTimeGhost = 3;
    [SerializeField] float distance_; 
    private int randomSpotGhost;
    Vector3 startPositionGhost;
    [SerializeField] Animator anime;
    Rigidbody2D Rigid;
    private void Awake()
    {
        startPositionGhost = transform.position;
    }
    void Start()
    {
        Rigid = GetComponent<Rigidbody2D>();
        this.speed = 1;
        timerGhost = 0;
        waitTimeGhost = 3;
        randomSpotGhost = Random.Range(0, moveTarget.Length - 1);
        isGhostMoving = true;
        PlayerTargetGhost = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    // Update is called once per frame
    void Update()
    {
        distance_ = Vector3.Distance(transform.position, PlayerTargetGhost.transform.position);
        Debug.Log(distance_);
        Rigid.velocity = new Vector2(0, 0);
        if (distance_ > 8.0 && isGhostMoving)
        {
            //Debug.Log("isMoveing is true");
            transform.position = Vector2.MoveTowards(transform.position, moveTarget[randomSpotGhost].position, speed * Time.deltaTime);
        }
        else if(distance_ <= 8.0)
        {
            //Debug.Log("the player is near"); 
            transform.position = Vector2.MoveTowards(transform.position, PlayerTargetGhost.position, speed * Time.deltaTime); 
        }
        UpdateTimerGhost();
    }

    void UpdateTimerGhost()
    {
        timerGhost += Time.deltaTime;
        if (isGhostMoving)
        {
            if (timerGhost > moveTimeGhost)
            {
              //  Debug.Log("ismoving is now false");
                timerGhost = 0;

                isGhostMoving = false;
            }
        }
        if (!isGhostMoving)
        {
            if (timerGhost > waitTimeGhost)
            {
                randomSpotGhost = Random.Range(0, moveTarget.Length - 1);
                timerGhost = 0;
                isGhostMoving = true;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "flashlight")
        {
            anime.SetBool("LightUp", true);
        }
        if(col.gameObject.tag == "wall")
        {
            Physics2D.IgnoreCollision(col.collider,GetComponent<Collider2D>());
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        anime.SetBool("LightUp", false);
    }

}
