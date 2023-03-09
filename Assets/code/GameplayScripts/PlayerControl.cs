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
    [Header("Movement")]
    private Vector3 playerVelocity;
    [SerializeField] private float playerSpeed = 2.0f;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 1.0f;
    private float gravityValue = -9.80f;
    private float noJumpingTimer = 0.0f; //Used to prevent jump inputs in quick succession from allowing players to jump really high.
    [SerializeField] private float noJumpingTime = 2.2f; //This one is used to let us adjust the timing of the grce period in the editor.

    [SerializeField] GameObject AttackBox;
    [SerializeField] private LayerMask Layer_enemy;
    private BoxCollider AttackCollider;
    private PlayerMana MyMana;
    private GameObject ObjInFrontOfPlayer;
    public bool IsAttacking = false;
    [SerializeField] private Transform cameraTransform;

    //[Header("References-Climing")]
    //public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;
    private bool B_Inf_Climbing;
    //Anims
    private Animator _Anim;
    //States
    StateMachine _StateMachine;
    public string statename;
    protected class GroundState : BaseState
    {
        PlayerControl player;
        public GroundState(PlayerControl player, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.player = player;
        }
        public override void OnEnter()
        {
            player._Anim.SetBool("IsGround", true);
            player.climbTimer = player.maxClimbTime;
        }
        public override void Update()
        {
            player.Move();
            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && player.CheckGrounded() && player.noJumpingTimer <= 0.0f)
            {
                player._Anim.SetTrigger("JumpReady");
                player.noJumpingTimer = player.noJumpingTime; Debug.Log("noJumpingTimer == " + player.noJumpingTimer);
                player.StartCoroutine(player.WaitBeforeJump());
            }
            if (player.noJumpingTimer >= 0.0f) { player.noJumpingTimer -= Time.deltaTime; Debug.Log("noJumpingTimer == " + player.noJumpingTimer); }
        }
    }
    protected class AirState : BaseState
    {
        PlayerControl player;
        public AirState(PlayerControl player, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.player = player;
        }
        public override void OnEnter()
        {
            player._Anim.SetBool("IsGround", false);
        }
        public override void Update()
        {
            player.Move();
        }
    }
    protected class ClimbingState : BaseState
    {
        PlayerControl player;
        public ClimbingState(PlayerControl player, string name, StateMachine stateMachine) : base(name, stateMachine)
        {
            this.player = player;
        }
        public override void OnEnter()
        {
            player._Anim.SetTrigger("ClimbingTrigger");
            player._Anim.SetBool("IsClimbing", true);
        }
        public override void Update()
        {

            if (!player.B_Inf_Climbing) player.climbTimer -= Time.deltaTime;
            player.controller.Move(new Vector3(0, player.climbSpeed*Time.deltaTime,0));
            //if (!player.CheckClimableWall()) stateMachine.ChangeState(player.State_Air);
            if (player._ControlInputs.jump) stateMachine.ChangeState(player.State_Air);
            if (!player.B_Inf_Climbing &&player.climbTimer < 0) stateMachine.ChangeState(player.State_Air);
        }
        public override void OnExit()
        {
            player._Anim.SetBool("IsClimbing", false);
        }
    }
    protected class C_IsGround : TransactionCondition
    {
        protected PlayerControl player;
        public C_IsGround(PlayerControl player)
        {
            this.player = player;
        }
        public override bool TriggerCheck()
        {
            return player.CheckGrounded();
        }
    }
    private struct ControlInputs{
        public bool jump,jump_hold;
        public float axis_Horizontal, axis_Vertical;
        public bool run;
        public bool interact, interact_hold;
    }
    private ControlInputs _ControlInputs = new ControlInputs();
    private BaseState State_Ground, State_Air, State_Climbing;
    private Transaction Tr_Ground_Air, Tr_Air_Ground;
    private bool CheckGrounded() //Check is on ground with ray, this prevent player can not jump while on tilt ground
    {
        if (controller.isGrounded) { return true; } //This is very odd, sometimes return false even it looks touch the ground
        var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
        var tolerance = 0.3f;
        return Physics.Raycast(ray, tolerance);
    }
    private bool CheckClimableWall() //Check Player's front wall are climable
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
        _Anim.SetBool("Attack", true);
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
        _Anim.SetBool("Attack", false);
        IsAttacking = false; 
    }
    IEnumerator WaitBeforeJump()
    {
        _Anim.SetBool("HasJumped", true); 
        yield return new WaitForSeconds(0.48f);
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        _Anim.SetBool("HasJumped", false);
    }
    private void GetInputs()
    {
        _ControlInputs.jump = Input.GetButtonDown("Jump");
        _ControlInputs.jump_hold = Input.GetButton("Jump");
        _ControlInputs.axis_Horizontal = Input.GetAxis("Horizontal");
        _ControlInputs.axis_Vertical = Input.GetAxis("Vertical");
        _ControlInputs.run = Input.GetButton("Sprint");
        _ControlInputs.interact = Input.GetButtonDown("Interact");
        _ControlInputs.interact_hold = Input.GetButton("Interact");
    }
    private void Walk(Vector3 move)
    {
        _Anim.SetFloat("Speed", 1.0f, 0.1f, Time.deltaTime);
        controller.Move(move * Time.deltaTime * playerSpeed);
    }
    private void Run(Vector3 move)
    {
        if(MyMana.mana > 0)
        {
            controller.Move(move * Time.deltaTime * (playerSpeed * 3));
            _Anim.SetFloat("Speed", 2, 0.1f, Time.deltaTime);
            MyMana.DecreasedMana(10.0f * Time.deltaTime);
        }
        else
        {
            _Anim.SetFloat("Speed", 1.2f, 0.1f, Time.deltaTime);
            controller.Move(move * Time.deltaTime * playerSpeed); 
        }
    }
    private void Move()
    {
        Vector3 move = new Vector3(_ControlInputs.axis_Horizontal, 0, _ControlInputs.axis_Vertical);
        Quaternion cameraRotation = cameraTransform.transform.rotation;
        cameraRotation.x = 0;
        cameraRotation.z = 0;
        move = cameraRotation * move * playerSpeed;
        if (move == Vector3.zero)
        {
            _Anim.SetFloat("Speed", 0.0f, 0.1f, Time.deltaTime);
        }
        else
        {
            gameObject.transform.forward = move;
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                Walk(move);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                Run(move);
            }
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        if (CheckClimableWall())
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                _StateMachine.ChangeState(State_Climbing);
                return;
            }
        }
        if (CheckGrounded() && playerVelocity.y < 0) //Ensures that the player stops when hitting the ground
        {
            _StateMachine.ChangeState(State_Ground);
            playerVelocity.y = 0f;
        }
    }
    private void Start()
    {
        _StateMachine = gameObject.AddComponent<StateMachine>();

        State_Ground = new GroundState(this, "Ground", _StateMachine);
        State_Air = new AirState(this, "Air", _StateMachine);
        State_Climbing = new ClimbingState(this, "Climbing", _StateMachine);

        Tr_Ground_Air = new Transaction(State_Air);
        Tr_Air_Ground = new Transaction(State_Ground);

        TransactionCondition c_IsGround = new C_IsGround(this);

        Tr_Ground_Air.addCondition(c_IsGround, true);
        Tr_Air_Ground.addCondition(c_IsGround, false);

        State_Ground.addTransaction(Tr_Ground_Air);
        
        _StateMachine.setInitState(State_Ground);

        B_Inf_Climbing = (maxClimbTime <= 0);

        controller = this.gameObject.GetComponent<CharacterController>();
        _Anim = this.gameObject.GetComponent<Animator>();
        AttackCollider = AttackBox.GetComponent<BoxCollider>();
        MyMana = this.gameObject.GetComponent<PlayerMana>();
    }
    void Update()
    {
        GetInputs();
        _StateMachine.Update();
        statename = _StateMachine.currentState.name;
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PunchAttack(AttackCollider));
        }
    }
    
    private void FixedUpdate()
    {
    }
}
