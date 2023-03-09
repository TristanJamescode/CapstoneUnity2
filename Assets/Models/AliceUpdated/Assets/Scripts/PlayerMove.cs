using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float speed, rotationSpeed; // speed 
    public Rigidbody2D rb; // Rigidbody 

    private Vector3 mousePos; // mouse potiions
    private Vector3 thisPos; // position of the player in relation to the camera and mouse
    private float angle; // angle in which to rotate

    
    public GameManager gameManager;
    // player Base Speed
    [SerializeField]
    float baseSpeed;

    // timer for invinceblity
    [SerializeField]
    float invincbleTimer;

    // player is Invincble
    [SerializeField]
    bool isInvincble;

    // timer for invincebilty
    [SerializeField]
    float powerupTimer;

    // speed change on powerup
    [SerializeField]
    float speediff;

    public int PlayHealth;//min player health
    private int maxPlayHealth = 3;//max player health
    public Image[] healtharray;// array health
    public Sprite fullpillow; //Sprite for full pillow
    public Sprite emptypillow;//Sprite for empty pillow
    Menu men;
    public batterylife batterylife; // battery life


    // Start is called before the first frame update
    void Start()
    {
        PlayHealth = maxPlayHealth;



        rb = this.gameObject.GetComponent<Rigidbody2D>();
        if (baseSpeed <= 0)
        {
            baseSpeed = 10;
            Debug.Log("Basespeed <= 0, set to 10");
        }
        if (speed < baseSpeed) // fail case 
        {
            speed = 10;
            Debug.Log("Speed less than base speed, set to baseSpeed");
        }
        if (rotationSpeed <= 0) // fail case 
        {
            rotationSpeed = 10;
            Debug.Log("rotationSpeed less than 0, set to 10");
        }
        if (!rb) // fail case
        {
            Debug.Log("No Rigidbody add!");
        }
    }


    void Update()
    {
        {
            // 
            if (PlayHealth > maxPlayHealth)
            {

                PlayHealth = maxPlayHealth;
            }

            // if  i  is less then the health bar array less then i incerments
            for (int i = 0; i < healtharray.Length; i++)
            {
                // if  i  is less then the PlayHealth then the sprite is fullpillow else its an empty pillow
                if (i < PlayHealth)
                {
                    healtharray[i].sprite = fullpillow;
                }
                else
                {
                    healtharray[i].sprite = emptypillow;

                }
                // if  i is less then the max health then the sprite is active else not active 
                if (i < maxPlayHealth)
                {
                    healtharray[i].enabled = true;

                }
                else
                {
                    healtharray[i].enabled = false;
                }
            }

            //Debug.Log(PlayHealth);
            if (invincbleTimer > 0)
            {
                invincbleTimer -= Time.deltaTime;
            }
            else
            {
                isInvincble = false;
            }
            Death();
        }
    }
        // Update is called once per frame
        void FixedUpdate()
        {
            rb.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * speed, 0.8f), Mathf.Lerp(0, Input.GetAxis("Vertical") * speed, 0.8f)); // player movement

            // rotation code
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            ReturnToBase();
        }


        //player interacts (powers ups and enemys)
        void OnTriggerEnter2D(Collider2D col)
        {


            if (col.gameObject.tag == "Battery")
            {
                // Debug.Log("sap");
                batterylife.minbatterypower += 10;
                Destroy(col.gameObject);

            }

            if (col.gameObject.tag == "Powerup")
            {
                invincbleTimer = 3.0f;
                isInvincble = true;
                Destroy(col.gameObject);
            }

            if (col.gameObject.tag == "HealthUp")
            {
                if (PlayHealth < maxPlayHealth)
                {
                    PlayHealth++;
                }
                Destroy(col.gameObject);
            }

            if (col.gameObject.tag == "SpeedDown")
            {
                Debug.Log("Runner");
                speed -= 5;
                Destroy(col.gameObject);
            }

            if (col.gameObject.tag == "SpeedUp")
            {
                speed += 5;
                Destroy(col.gameObject);
            }

            if (col.gameObject.tag == "badguy" && !isInvincble)
            {
            PlayHealth--;
            }
        }
         void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag == "badguy" && !isInvincble)
            {
                Debug.Log("hurt");
                PlayHealth -= 1;
            }
            if (col.gameObject.tag == ("EndGame"))
            {
                gameManager.switchWorlds();
                Debug.Log("endgame");
            }
            if(col.gameObject.tag == "SendToWin")
        {
            SceneManager.LoadScene("WinGame");
        }
        }

        #region return speed to base speed
        void ReturnToBase()
        {
            if (speed > baseSpeed)
            {
                speed -= Time.deltaTime;
            }
            else if (speed < baseSpeed)
            {
                speed += Time.deltaTime;
            }
        }
        #endregion

        void Death()
        {
            if (PlayHealth <= 0)
            {
                SceneManager.LoadScene("GameOver");

            }


        } 



}
