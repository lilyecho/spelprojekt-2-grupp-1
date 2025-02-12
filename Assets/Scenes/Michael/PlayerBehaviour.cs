using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    #region State Machines


    public State currentState;
    public JumpState jumpState;
    //public State walkState;
    public enum MovementMode { WALK, SNEAK, RUN};
    public MovementMode movementMode; 


    #endregion


    #region Behaviour States

    public State idle;
    public State walking;
    public State sneaking;
    public State running;
    public State jumping;
    public State falling;
    public State gliding;
    public State onTree;
    public State wallJumping;

    #endregion


    #region Jump States
    public JumpState ableToJump;
    public JumpState unableToJump;
    public JumpState chargingJump;
    public JumpState jumpCharged;
    #endregion



    //Patrik pillar lite
    [SerializeField] private PlayerMovementData playerMovementData; 
    //Patrik pillar inte mer
    
    public Rigidbody rb;
    

    [HideInInspector]
    public Vector2 moveInput;
    [HideInInspector]
    public Vector3 moveDir;

    

    Camera cam;
    Vector3 cameraForward;
    Vector3 cameraRight;


    [HideInInspector]
    public float moveSpeed;
    
    public Transform[] rayCastPoints = new Transform[4];
    public float rayCastLength;



    [HideInInspector]public float accTime;



    [HideInInspector]
    public bool intoJump = false;
    [HideInInspector]
    public bool intoChargingJump = false;

    /*
    public Transform leftFontPaw;
    public Transform rightFontPaw;
    public Transform leftBackPaw;
    public Transform rightBackPaw;
    */

    public PlayerMovementData GetMovementData => playerMovementData;
    
    void Start()
    {
        ableToJump = new AbleToJump(this);
        unableToJump = new UnableToJump(this);
        chargingJump = new ChargingJump(this);
        jumpCharged = new JumpCharged(this);

        idle = new Idle(this);
        walking = new Walking(this);
        sneaking = new Sneaking(this);
        running = new Running(this);
        jumping = new Jumping(this);
        falling = new Falling(this);

        jumpState = ableToJump;
        //walkState = walking;
        movementMode = MovementMode.WALK;
        currentState = idle;

        cam = Camera.main;


    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Update();
        jumpState?.Update();
        //moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        cameraForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        cameraRight = new Vector3(cam.transform.right.x, 0, cam.transform.right.z);
        cameraForward.Normalize();
        cameraRight.Normalize();

        moveDir = (moveInput.x * cameraRight + moveInput.y * cameraForward).normalized;

    }

    private void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void ChangeState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void ChangeJumpState(JumpState newState)
    {
        jumpState?.Exit();
        jumpState = newState;
        jumpState.Enter();
    }

    public void ChangeWalkState(State newState)
    {
        /*walkState?.Exit();
        walkState = newState;
        walkState.Enter();*/
    }

    public void Space(InputAction.CallbackContext context)
    {
        currentState?.OnSpaceBar(context);
        jumpState?.OnSpaceBar(context);
    }

    public void Shift(InputAction.CallbackContext context)
    {
        
        //walkState?.OnShift(context);
        if(context.performed)
        {
            movementMode = MovementMode.SNEAK;
        }
        if (context.canceled)
        {
            movementMode = MovementMode.WALK;
        }
        currentState?.OnShift(context);
    }

    public void CTRL(InputAction.CallbackContext context)
    {
        
        //walkState?.OnCTRL(context);
        if (context.performed)
        {
            movementMode = MovementMode.RUN;

        }
        if (context.canceled)
        {
            movementMode = MovementMode.WALK;
        }
        currentState?.OnCTRL(context);
    }

    public void WASD(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        if (context.canceled)
        {
            moveInput = Vector2.zero;
            accTime = 0;
        }


        currentState?.OnWASD(context);

    }

    public void Mouse(InputAction.CallbackContext context)
    {
        //currentState?.OnSpaceBar(context);
    }


    private void OnCollisionEnter(Collision collision)
    {
        currentState?.OnCollision(collision);
    }


    public float rotationSpeed = 10f;

    public void RotateCharacter(Vector3 moveDir)
    {
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }


    


}
