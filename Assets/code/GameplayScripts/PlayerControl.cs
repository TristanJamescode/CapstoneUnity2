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
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] GameObject AttackBox;
    [SerializeField] private LayerMask Layer_enemy;
    private BoxCollider AttackCollider; 
    private Animator Anim;
    public bool IsAttacking = false; 
    [SerializeField] private Transform cameraTransform;
    private void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
        Anim = this.gameObject.GetComponent<Animator>();
        AttackCollider = AttackBox.GetComponent<BoxCollider>(); 
    }

    private bool CheckGrounded() //Check is on ground with ray, this prevent player can not jump while on tilt ground
    {
        if (controller.isGrounded) { return true; } //This is very odd, sometimes return false even it looks touch the ground
        var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
        var tolerance = 0.3f;
        return Physics.Raycast(ray, tolerance);
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

    private void Run(Vector3 move)
    {
        controller.Move(move * Time.deltaTime * (playerSpeed * 3));
        Anim.SetBool("IsRunning", true); 
    }

    void Update()
    {
        if (CheckGrounded() && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PunchAttack(AttackCollider)); 
        }

        if(IsAttacking)
        {
            //AttackCollider
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Quaternion cameraRotation = cameraTransform.transform.rotation;
        cameraRotation.x = 0;
        cameraRotation.z = 0;
        move = cameraRotation * move;
        if(!Input.GetKey(KeyCode.R))
        {
            controller.Move(move * Time.deltaTime * playerSpeed);
            if(Anim.GetBool("IsRunning") == true)
            {
                Anim.SetBool("IsRunning", false); 
            }
        }
        else if (Input.GetKey(KeyCode.R))
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
}
