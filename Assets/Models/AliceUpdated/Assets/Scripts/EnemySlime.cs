using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : MonoBehaviour
{
    float moveTime = 3;
    float orginOffset = 0.5f; // raycast offset 
    float waitTime = 3;
    public float timer, chaseTimer, chaseTimerMax, speed, chaseSpeed;
    bool isMoving;
    int randomSpot;
    public AudioSource soundPlayer;
    public AudioClip enemyNoise;
    Rigidbody2D rb;
    public Transform[] moveTarget;
    public Transform PlayerTarget;
    [SerializeField] bool onChase;

    [SerializeField] Animator anim;

    Vector3 startPosition;
    private void Awake()
    {
        startPosition = transform.position;
        soundPlayer.clip = enemyNoise;
    }
    void Start()
    {

        
        if(chaseTimerMax <= 0.0f)
        {
            Debug.Log("ChaseMax not set, Set to 4");
            chaseTimerMax = 4.0f;
        }
        if(speed <= 0.0f)
        {
            Debug.Log("speed not set, Set to 1");
            speed = 1;
        }
        if(chaseSpeed <= 0.0f)
        {
            Debug.Log("chaseSpeed not set, Set to 1.1f");
            chaseSpeed = 1.1f;
        }
        chaseTimer = 0;
        timer = 0;
        randomSpot = 0;
        isMoving = true;
        onChase = false;

        rb = GetComponent<Rigidbody2D>();
        PlayerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void PlaySound()
    {
        soundPlayer.Play();
        soundPlayer.loop = false; 
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(transform.position, PlayerTarget.transform.position));
    
        onChase = IsPlayerNear();
        if (onChase)
        {
            PlaySound(); 
            transform.position = Vector3.MoveTowards(transform.position, PlayerTarget.position, chaseSpeed * Time.deltaTime);
        }
        else
        {
            if (isMoving)
            {
                // Debug.Log("isMoveing is true");
                transform.position = Vector3.MoveTowards(transform.position, moveTarget[randomSpot].position, speed * Time.deltaTime);
            }
        }
        rb.velocity = new Vector2(0, 0);
        UpdateTimer();
        UpdatePatrolPosition();
    }

    #region Detect Objects with Raycast
    public RaycastHit2D CheckRaycast(Vector2 direction) // this will send a ray cast in a direction and return what it has hit
    {
        RaycastHit2D sender; // ray cast var 
        float directionOffset = orginOffset * (direction.x > 0 || direction.y > 0 ? 1 : -1); // mutliplys offset either by 1 or -1 based on direction 
        if (direction == Vector2.up || direction == Vector2.down) // if the ray is vertical
        {
             sender = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + directionOffset), direction);
            Debug.DrawRay(new Vector2(transform.position.x , transform.position.y + directionOffset), direction, Color.red);
        }
        else // ray cast call is horizontal
        {
             sender = Physics2D.Raycast(new Vector2(transform.position.x + directionOffset, transform.position.y ), direction);
            Debug.DrawRay(new Vector2(transform.position.x + directionOffset, transform.position.y), direction, Color.red);
        }
        return sender; // return raycast object
    }
    #endregion

    #region Is Player Near By
    public bool IsPlayerNear()
    {
        if(Vector2.Distance(transform.position,PlayerTarget.position) <= 4) // if the player is 4 units away from this bad guy
        {
            RaycastHit2D hitUP = CheckRaycast(Vector2.up); // checks ray up
            RaycastHit2D hitRight = CheckRaycast(Vector2.right); // checks Ray right
            RaycastHit2D hitLeft = CheckRaycast(Vector2.left); // checks Ray Left
            RaycastHit2D hitDown = CheckRaycast(Vector2.down); // checks Ray down
        
            // Debug.Log(hitUP.collider.name);
            // if the player is what the ray cast hits then return true and begin chase
            if(hitUP.collider.name == "Player" || hitRight.collider.name == "Player" || hitDown.collider.name == "Player" || hitLeft.collider.name  == "Player")
            {
                return  true;
            }
        }
        return false;
    }
    #endregion

    #region Update Tracker Position 
    void UpdatePatrolPosition()
    {
        if (transform.position.x <= moveTarget[randomSpot].position.x + 0.2 &&
        transform.position.x >= moveTarget[randomSpot].position.x - 0.2 &&
        transform.position.y <= moveTarget[randomSpot].position.y + 0.2 &&
        transform.position.y >= moveTarget[randomSpot].position.y - 0.2)    
        // will complete the movement to a point before switching targets
        {
            if (randomSpot < moveTarget.Length - 1)
            {
                randomSpot++;
            }
            else
            {
                randomSpot = 0;
            }
        }
    }
    #endregion

    #region Update Movement Timer

    void UpdateTimer()
    {
        timer += Time.deltaTime;
        if(onChase) { // if the on chase then will time to chase the player
            chaseTimer += Time.deltaTime;
            if (chaseTimer > chaseTimerMax)
            {
                chaseTimer = 0;
                onChase = false;
            }
        }
        if (isMoving)
        {
            if (timer > moveTime)
            {
               // Debug.Log("ismoving is now false");
                timer = 0;
                isMoving = false;
            }
        }
        if (!isMoving)
        {
            if (timer > waitTime)
            {
                timer = 0;
                isMoving = true;
            }
        }
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D hit)
    {
       //ebug.Log("asdfasfasfdsdaf");
        if(hit.gameObject.tag == "flashlight")
        {
            anim.SetBool("LightUp", true);
            soundPlayer.clip = enemyNoise;
            soundPlayer.PlayOneShot(enemyNoise);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("LightUp", false);
    }
}
