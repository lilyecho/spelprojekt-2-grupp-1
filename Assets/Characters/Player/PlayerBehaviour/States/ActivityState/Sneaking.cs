using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sneaking : State, IAcceleration
{
    public Sneaking(PlayerBehaviour playerBehaviour) : base(playerBehaviour)
    {

    }
    
    private float timeStep = .5f;
    private float currentTime = 0;
    
    
    float time = 0f;
    Vector3 normal;


    bool coyote = false;
    float coyoteTimer;
    public override void Enter()
    {
        OnEnterChangeGlobalActivityParameter(playerBehaviour.GetParameterData.GetCatSneak, (int)CharacterActivity.Sneak);
        Debug.Log("SNEAKING");
        //playerBehaviour.moveSpeed = playerBehaviour.GetMovementData.GetSneakSpeed;
        playerBehaviour.anim.SetBool(Animator.StringToHash("Sneaking"), true);
    }
    
    public override void Exit()
    {
        playerBehaviour.anim.SetBool(Animator.StringToHash("Sneaking"), false);
    }

    public override void OnCollision(Collision collision)
    {

    }

    public override void Update()
    {
        
        normal = GetSurfaceNormal(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength * 2);
        //playerBehaviour.transform.rotation = AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, time, Vector3.up);
        playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, normal,
                                                                playerBehaviour.GetMovementData.GetSlopeCheckerLength, playerBehaviour.GetMovementData.GetMaxRotationAngle), time);
        time = time + Time.deltaTime;


        if (!coyote && !CheckForGround(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength * 1.5f))
        {
            //playerBehaviour.ChangeState(playerBehaviour.falling);
            coyote = true;
        }



        if (coyote)
        {
            coyoteTimer -= Time.deltaTime;
            if (CheckForGround(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength * 1.5f))
            {
                coyote = false;
                coyoteTimer = coyoteTimer = playerBehaviour.GetMovementData.GetCoyoteTime;
            }
            if (coyoteTimer <= 0)
            {
                playerBehaviour.ChangeState(playerBehaviour.falling);
            }
        }
        
        //TODO Sound temporary
        currentTime += Time.deltaTime;
        if (currentTime >= timeStep)
        {
            TakeStep();
            currentTime = 0;
        }
    }

    public override void FixedUpdate()
    {
        
        playerBehaviour.RotateCharacter(playerBehaviour.moveDir);
        playerBehaviour.moveDir = Vector3.ProjectOnPlane(playerBehaviour.moveDir, normal).normalized;

        ApplyAcceleration(playerBehaviour.GetMovementData.GetSpeedRelated.sneak.speed,playerBehaviour.GetMovementData.GetSpeedRelated.sneak.accTotalTime);
        
    }
    
    public override void OnSpaceBar(InputAction.CallbackContext context)
    {

    }
    public override void OnShift(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerBehaviour.ChangeState(playerBehaviour.walking);
        }
    }
    public override void OnCTRL(InputAction.CallbackContext context)
    {

    }

    public override void OnWASD(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerBehaviour.ChangeState(playerBehaviour.idle);
        }
    }
    public override void OnMOUSE(InputAction.CallbackContext context)
    {

    }
    /*
    public bool CheckForGround()
    {
        foreach (Transform t in playerBehaviour.rayCastPoints)
        {
            if (Physics.Raycast(t.position, Vector3.down, playerBehaviour.rayCastLength))
            {
                return true;
            }
        }
        return false;
    }
    */
    public void FixCurrentAccelerationTime()
    {
        float currentSpeed = playerBehaviour.rb.velocity.magnitude;
        float maxSpeed = playerBehaviour.GetMovementData.GetSpeedRelated.sneak.speed;
        float totalAccelerationTime = playerBehaviour.GetMovementData.GetSpeedRelated.sneak.accTotalTime;
        playerBehaviour.accTime = CalculateAccelerationTimeFromSpeed(currentSpeed,maxSpeed,totalAccelerationTime);
    }
}
