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
    [SerializeField] float moveSpeed_Max = 5;
    [SerializeField] float moveSpeed = 0.7f;
    [SerializeField] float Jumpforce = 10;
    [SerializeField] float GravityS = 1;
    [SerializeField] GameObject FireParticles; 

    private float moveSpeedCurrent = 0;

    private CharacterController control;
    private Vector3 Movedir;

    private Animator anim;

    private enum PLAYERSTATE
    {
        IDLE,
        WALK,
        RUN,
        JUMP,
        INAIR,
        LAND,
        ATTACK
    }
    //private PLAYERSTATE playerState = PLAYERSTATE.IDLE;
    private bool isGround = true;

    //Inputs
    private float moveX = 0;
    private float moveZ = 0;
    private bool jump = false;

    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        control = this.gameObject.GetComponent<CharacterController>();
        FireParticles = GameObject.FindGameObjectWithTag("Fire"); 
    }

    private void Update()
    {
        GetInputs();

        Move();
    }

    private void ShootFire()
    {
        FireParticles.SetActive(true); 
    }
    private void GetInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
        jump = Input.GetButtonDown("Jump");
    }

    private void Move()
    {
        isGround = CheckGrounded();
        //Debug.Log(isGround);
        if (moveX != 0 || moveZ != 0) //if inputing
        {
            if (moveSpeedCurrent < moveSpeed_Max) moveSpeedCurrent += moveSpeed * Time.deltaTime;

            Vector3 direction = new Vector3(moveX, 0f, moveZ).normalized; //direction walk
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f); //rotate Player direction
            Movedir = (direction * moveSpeedCurrent);

        }
        else
        {
            moveSpeedCurrent = 0;
            Movedir.x = 0;
            Movedir.z = 0;
        }
        anim.SetFloat("SpeedParam", moveSpeedCurrent / moveSpeed_Max);
        Movedir.y += (Physics.gravity.y * GravityS); //gravity
        if (isGround) //if the player is on Ground
        {
            Movedir.y = -0.1f;
            if (jump) Jump();
            anim.SetBool("IsAir", false);
        }
        else
        {
            anim.SetBool("IsAir", true);
        }
        control.Move(Movedir * Time.deltaTime);
    }

    private void Jump()
    {
        Debug.Log("jump");
        Movedir.y = Jumpforce;
        anim.SetTrigger("Jump");
    }

    private bool CheckGrounded() //Check is on ground with ray, this prevent player can not jump while on tilt ground
    {
        if (control.isGrounded) { return true; } //This is very odd, sometimes return false even it looks touch the ground
        var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
        var tolerance = 0.3f;
        return Physics.Raycast(ray, tolerance);
    }
}
