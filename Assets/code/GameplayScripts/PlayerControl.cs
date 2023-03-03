using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// References
/// https://www.youtube.com/watch?v=QCU3254Rk8I
/// </summary>
public class PlayerControl : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    private float gravityValue = -9.80f;
    [SerializeField] GameObject AttackBox;
    [SerializeField] private LayerMask Layer_enemy;
    private BoxCollider AttackCollider; 
    private Animator Anim;
    private PlayerMana MyMana;
    private GameObject ObjInFrontOfPlayer;
    private bool IsClimbing = false; 
    public bool IsAttacking = false; 
    [SerializeField] private Transform cameraTransform;
    private void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
        Anim = this.gameObject.GetComponent<Animator>();
        AttackCollider = AttackBox.GetComponent<BoxCollider>();
        MyMana = this.gameObject.GetComponent<PlayerMana>(); 
    }

    private bool CheckGrounded() //Check is on ground with ray, this prevent player can not jump while on tilt ground
    {
        if (controller.isGrounded) { return true; } //This is very odd, sometimes return false even it looks touch the ground
        var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
        var tolerance = 0.3f;
        return Physics.Raycast(ray, tolerance);
    }

    private bool CheckClimableWall()
    {
        RaycastHit hit; 

        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1.0f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            ObjInFrontOfPlayer = hit.transform.gameObject; 
            if(ObjInFrontOfPlayer.tag == "SteelWall")
            {
                Debug.Log("This wall can be climbed");
                return true;
            }
            else
            {
                Debug.Log("This wall cannot be clibed");
                return false; 
            }
        }
        return false; 
    }

    IEnumerator PunchAttack(Collider AttackCollider)
    {
        Anim.SetBool("Attack", true);
        IsAttacking = true;
        Collider[] cols = Physics.OverlapBox(AttackCollider.bounds.center, AttackCollider.bounds.extents,
           AttackCollider.transform.rotation, Layer_enemy); 
        foreach(Collider c in cols)
        {
            Debug.Log(c.gameObject.name);
            c.GetComponentInParent<BasicEnemy>().Take_Damage(50);
            c.GetComponentInParent<BasicEnemy>().Take_Knockback(2, c.gameObject.transform.position-this.transform.position);
        }
        yield return new WaitForSeconds(1.90f);
        Anim.SetBool("Attack", false);
        IsAttacking = false; 
    }

    IEnumerator WaitBeforeJump()
    {
        Anim.SetBool("HasJumped", true); 
        yield return new WaitForSeconds(0.48f);
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        yield return new WaitForSeconds(1.82f);
        Anim.SetBool("HasJumped", false); 
    }

    private void Climb()
    {

    }

    private void Run(Vector3 move)
    {
        if(MyMana.mana > 0)
        {
            controller.Move(move * Time.deltaTime * (playerSpeed * 3));
            Anim.SetBool("IsRunning", true);
            MyMana.DecreasedMana(10.0f * Time.deltaTime);
        }
        else
        {
            Anim.SetBool("IsRunning", false); 
            Anim.SetBool("IsWalking", true);
            controller.Move(move * Time.deltaTime * playerSpeed); 
        }
    }

    void Update()
    {
        if(CheckClimableWall())
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                IsClimbing = true;
                Anim.SetBool("IsClimbing", true); 

            }
        }

        if (CheckGrounded() && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PunchAttack(AttackCollider)); 
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Quaternion cameraRotation = cameraTransform.transform.rotation;
        cameraRotation.x = 0;
        cameraRotation.z = 0;
        move = cameraRotation * move;
        if(!Input.GetKey(KeyCode.LeftShift))
        {
            controller.Move(move * Time.deltaTime * playerSpeed);
            if(Anim.GetBool("IsRunning") == true)
            {
                Anim.SetBool("IsRunning", false); 
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            Run(move); 
        }
        

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Anim.SetBool("IsWalking", true);
        }
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            Anim.SetBool("IsWalking", false);
        }

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && CheckGrounded())
        {
            StartCoroutine(WaitBeforeJump()); 
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        
    }
}
