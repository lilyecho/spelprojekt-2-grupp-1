using System;
using Characters.Player.PlayerBehaviour;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerBehaviour : MonoBehaviour
{
    #region State Machines
    
    public State currentState;
    public JumpState jumpState;
    //public State walkState;
    public enum MovementMode { WALK, SNEAK, RUN};
    public MovementMode movementMode; 
    
    #endregion

    [SerializeField] private EnemyManagerPort enemyManagerPort = null;
    [SerializeField] private CheckPointPort checkPointPort = null;
    
    [Space,SerializeField] private PlayerData playerDataNormal;
    [SerializeField] private PlayerData playerDataShrink;
    private PlayerData _currentPlayerPlayerData;
    
    [SerializeField] private AbilityData.Abilities currentAbilities;
    
    #region Audio

    [Header("Audio-Stuff")]
    [SerializeField] private AudioPort audioPort;
    
    public AudioPort GetAudioPort => audioPort;

    #endregion
    [Space]
    
    public Rigidbody rb;
    [HideInInspector] public Animator anim;    
    
    [HideInInspector]
    public Vector2 moveInput;
    [HideInInspector]
    public Vector3 moveDir;
    
    Camera cam;
    Vector3 cameraForward;
    Vector3 cameraRight;
    public KameraPrototyp kameraPrototyp;

    [HideInInspector]
    public float moveSpeed;
    
    public Transform[] rayCastPoints = new Transform[4];
    public float rayCastLength;
    
    [HideInInspector]public float accTime;
    [HideInInspector] public bool intoJump = false;
    
    [Space, Header("Particles")]
    public ParticleSystem jumpParticles;
    public ParticleSystem megaJumpParticles;
    private ParticleSystem jumpParticlesInstance;
    
    [SerializeField] private TimeManager timeManager = null;
    private bool _movementOn = true;

    //test
    public float minDistToTroll = 5;
    //

    //Hidden
    [SerializeField]private bool hidden;
    [SerializeField]private bool isAttacked;

    #region TimerRespawn
    [Space, Header("Respawn")]
    [SerializeField, Range(0.1f, 1)] private float respawnTimerCooldown;
    [SerializeField] private float _currenRespawnTime;

    #endregion
    
    #region Shrink
    [Space,Header("Shrink")]
    [SerializeField] private ParticleSystem particleSystemOnShrink;
    
    public ParticleSystem GetOnShrinkParticleSystem => particleSystemOnShrink;

    [SerializeField] private float shrinkCooldown = 1f;
    private float _shrinkCooldownTimer;

    #endregion

    #region Behaviour States

    [Header("Activity-states")]
    public Idle idle = new Idle();
    public Walking walking = new Walking();
    public Sneaking sneaking = new Sneaking();
    public Running running = new Running();
    public Jumping jumping = new Jumping();
    public Falling falling = new Falling();
    public Gliding gliding = new Gliding();
    public Shrink shrink = new Shrink();

    #endregion
    
    #region Jump States
    [Header("Jump-states")]
    public UnableToJump unableToJump = new UnableToJump();
    public NormalJump normalJump = new NormalJump();
    public MegaJump megaJump = new MegaJump();
    #endregion
    
    [Space]
    public bool debugStates;
    
    private void OnEnable()
    {
        timeManager.OnMovement += ChangeMovementActivation;
    }

    private void OnDisable()
    {
        timeManager.OnMovement -= ChangeMovementActivation;
    }

    private void Awake()
    {
        _currentPlayerPlayerData = playerDataNormal;
        
        falling.Awake(this);
        gliding.Awake(this);
        idle.Awake(this);
        jumping.Awake(this);
        running.Awake(this);
        shrink.Awake(this);
        sneaking.Awake(this);
        walking.Awake(this);
        
        //Jumps
        normalJump.Awake(this);
        megaJump.Awake(this);
        unableToJump.Awake(this);

        _shrinkCooldownTimer = shrinkCooldown;
    }

    private void OnDrawGizmos()
    {
        shrink.OnDrawGizmos(this);
        sneaking.OnDrawGizmos(this);
        walking.OnDrawGizmos(this);
        running.OnDrawGizmos(this);
        jumping.OnDrawGizmos(this);
    }
    private void OnDrawGizmosSelected()
    {
        shrink.OnDrawGizmosSelected(this);
        sneaking.OnDrawGizmosSelected(this);
        walking.OnDrawGizmosSelected(this);
        running.OnDrawGizmosSelected(this);
        jumping.OnDrawGizmosSelected(this);
    }

    /// <summary>
    /// True if movement is on
    /// </summary>
    /// <param name="nextValue"></param>
    public void ChangeMovementActivation(bool nextValue)
    {
        _movementOn = nextValue;
        rb.constraints = nextValue ? RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        anim.speed = nextValue ? 1 : 0;
        ChangeState(idle);
        
    }

    public EnemyManagerPort EnemyManagerPort => enemyManagerPort;
    
    public PlayerData GetShrinkPlayerData => playerDataShrink;
    public PlayerData GetNormalPlayerData => playerDataNormal;
    public PlayerData PlayerData
    {
        get => _currentPlayerPlayerData;
        set => _currentPlayerPlayerData = value;
    }

    //public AbilityData GetAbilityData => abilityData;
    public AbilityData.Abilities GetAbilities => currentAbilities;
    public AbilityData.Abilities ResetAbilities
    {
        set => currentAbilities = value;
    }
    
    public AbilityData.Abilities ChangeAbilities
    {
        set => currentAbilities |= value;
    }

    public bool Hidden
    {
        get => hidden;
        set => hidden = value;
    }

    public bool Attacked
    {
        get => isAttacked;
        set => isAttacked = value;
    }
    
    
    void Start()
    {
        jumpState = normalJump;
        
        cam = Camera.main;
        anim = GetComponent<Animator>();
        
        movementMode = MovementMode.WALK;
        StartInvoke(idle);
    }

    private void StartInvoke(State newState)
    {
        if (debugStates)
        {
            Debug.Log(newState);
        }
        currentState = newState;
        currentState.Enter();
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
        
        if (_currenRespawnTime > 0)
        {
            _currenRespawnTime -= Time.fixedDeltaTime;
        }

        if (_shrinkCooldownTimer > 0)
        {
            _shrinkCooldownTimer -= Time.fixedDeltaTime;
        }
        
        currentState?.FixedUpdate();
        
        SetSpeedParameterAnimation();
    }

    private void SetSpeedParameterAnimation()
    {
        //If low enough be zero - Specified request for animation
        float xzSpeed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        anim.SetFloat(Animator.StringToHash("Speed"), xzSpeed <= 0.001f ? 0 : xzSpeed);
    }

    public void ChangeState(State newState)
    {
        if (debugStates)
        {
            string t = "States";
            t += "\n PreState: "+currentState;
            t += "\n NextState: "+newState;
            Debug.Log(t);
        }
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void ChangeJumpState(JumpState newState)
    {
        if (debugStates)
        {
            string t = "JumpState";
            t += "\n PreState: "+jumpState;
            t += "\n NextState: "+newState;
            Debug.Log(t);
        }
        
        jumpState?.Exit();
        jumpState = newState;
        jumpState.Enter();
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

        if(GameManager.instance.runOnCTRL && movementMode != MovementMode.RUN)
        {
            if (context.performed)
            {
                movementMode = MovementMode.SNEAK;
            }
            if (context.canceled)
            {
                movementMode = MovementMode.WALK;
            }

            if (!_movementOn) return;
            currentState?.OnShift(context);
            jumpState?.OnShift(context);
        }

        else if(movementMode != MovementMode.SNEAK)
        {
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
    }

    public void CTRL(InputAction.CallbackContext context)
    {
        //walkState?.OnCTRL(context);
        if(GameManager.instance.runOnCTRL && movementMode != MovementMode.SNEAK)
        {
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

        else if(movementMode != MovementMode.RUN)
        {
            if (context.performed)
            {
                movementMode = MovementMode.SNEAK;
            }
            if (context.canceled)
            {
                movementMode = MovementMode.WALK;
            }

            if (!_movementOn) return;
            currentState?.OnShift(context);
            jumpState?.OnShift(context);
        }
        
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

    public void Shrink(InputAction.CallbackContext context)
    {
        if (_shrinkCooldownTimer <= 0 && context.performed)
        {
            Debug.Log("PlayerShrink");
            currentState?.OnShrink(context);
            _shrinkCooldownTimer = shrinkCooldown;
        }
    }

    
    public void Respawn(InputAction.CallbackContext context)
    {
        if (_currenRespawnTime > 0) return;
        
        if (context.performed)
        {
            _currenRespawnTime = respawnTimerCooldown;
            ChangeMovementActivation(false);
            checkPointPort.Respawn();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!_movementOn) return;
        currentState?.OnCollision(collision);
    }

    private void OnValidate()
    {
        idle?.OnValidate(this);
        walking?.OnValidate(this);
        sneaking?.OnValidate(this);
        running?.OnValidate(this);
        jumping?.OnValidate(this);
        falling?.OnValidate(this);
        gliding?.OnValidate(this);
        shrink?.OnValidate(this);
    }


    public float rotationSpeed = 10f;

    [HideInInspector]public float currentVelocity;
    [HideInInspector]public float smoothTime = 0.1f;

    [SerializeField] private float rotationTotalTime = 0.1f;
    private float rotationTimer = 0;
    private float testRot = 1000;
    
    
    public Quaternion RotateCharacter(Vector3 moveDir)
    {
        if (moveDir != Vector3.zero)
        {
            float targetAngle = MathF.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

            //transform.rotation = Quaternion.Euler(0, angle, 0);

            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

            return targetRotation;
        }
        return transform.rotation;
    }
    
    public void JumpParticles(ParticleSystem part)
    {
        jumpParticlesInstance = Instantiate(part, transform.position, jumpParticles.transform.rotation);
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.instance?.PauseGame();
        }
    }


}
