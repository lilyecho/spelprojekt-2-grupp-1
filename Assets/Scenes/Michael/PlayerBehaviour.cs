using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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

    
    [SerializeField] private PlayerMovementData playerMovementData;
    //[SerializeField] private AbilityData abilityData;
    [SerializeField] private AbilityData.Abilities currentAbilities;
    //TODO change when we get animations instead of a certain point 
    
    #region MaterialChecker

    [SerializeField] private Transform materialCheckerTransform = null;
    public Transform GetCheckerTransform => materialCheckerTransform;

    #endregion

    #region Audio

    [Header("Audio-Stuff")]
    [SerializeField] private AudioPort audioPort;
    [SerializeField] private FmodParameterData parameterData;
    [SerializeField] private CharacterAudioData characterAudioData;
    public AudioPort GetAudioPort => audioPort;
    public FmodParameterData GetParameterData => parameterData;
    public CharacterAudioData GetAudioData => characterAudioData;

    #endregion
    [Space]
    
    
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


    public ParticleSystem jumpParticles;
    private ParticleSystem jumpParticlesInstance;

    /*
    public Transform leftFontPaw;
    public Transform rightFontPaw;
    public Transform leftBackPaw;
    public Transform rightBackPaw;
    */

    [SerializeField] private TimeManager timeManager = null;
    private bool _movementOn = true;

    private void OnEnable()
    {
        timeManager.OnMovement += ChangeMovementActivation;
    }

    private void OnDisable()
    {
        timeManager.OnMovement -= ChangeMovementActivation;
    }

    private void ChangeMovementActivation(bool nextValue)
    {
        _movementOn = nextValue;
    }

    public PlayerMovementData GetMovementData => playerMovementData;
    //public AbilityData GetAbilityData => abilityData;
    public AbilityData.Abilities Abilities
    {
        get => currentAbilities;
        set => currentAbilities |= value;
    }

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
        if (!_movementOn) return;
        
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
        if (!_movementOn) return;
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
        if (!_movementOn) return;
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
        
        if (!_movementOn) return;
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
        
        if (!_movementOn) return;
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
        
        if (!_movementOn) return;
        currentState?.OnWASD(context);

    }

    public void Mouse(InputAction.CallbackContext context)
    {
        //currentState?.OnSpaceBar(context);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!_movementOn) return;
        currentState?.OnCollision(collision);
    }


    public float rotationSpeed = 10f;

    [HideInInspector]public float currentVelocity;
    [HideInInspector]public float smoothTime = 0.1f;

    [SerializeField] private float rotationTotalTime = 0.1f;
    private float rotationTimer = 0;
    private float testRot = 1000;
    
    
    public void RotateCharacter(Vector3 moveDir)
    {
        if (moveDir != Vector3.zero)
        {
            /*
            Debug.LogError("Rot"+Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z));
            float maxDegreeCurrent = (Quaternion.Angle(transform.rotation, targetRotation)/rotationSpeed)*Time.deltaTime;
            //Debug.LogError("Angles: "+testRot*Time.deltaTime);
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, testRot*Time.fixedDeltaTime);*/

            float targetAngle = MathF.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, angle, transform.eulerAngles.z);
            
        }
        
    }



    public void JumpParticles()
    {
        jumpParticlesInstance = Instantiate(jumpParticles, transform.position, jumpParticles.transform.rotation);
    }

}
