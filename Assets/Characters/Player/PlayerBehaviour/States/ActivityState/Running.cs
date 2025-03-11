using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[Serializable]
public class Running : State, IAcceleration
{
    [SerializeField] private bool gizmos;
    
    private float timeStep = .5f;
    private float currentTime = 0;
    
    float time = 0f;
    Vector3 normal;
    
    bool coyote = false;
    float coyoteTimer;
    public override void Enter()
    {
        //OnEnterChangeGlobalActivityParameter(playerBehaviour.GetParameterData.GetCatSneak, (int)CharacterActivity.Run);
        
        FixCurrentAccelerationTime();
    }
    public override void Exit()
    {

    }

    public override void OnCollision(Collision collision)
    {

    }

    public override void Update()
    {
        
        normal = GetSurfaceNormal(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength * 2);
        //playerBehaviour.transform.rotation = AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, time, Vector3.up);
        //playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, normal,
        //                                                        playerBehaviour.PlayerData.GetSlopeCheckerLength, playerBehaviour.PlayerData.GetMaxRotationAngle), time);
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
                coyoteTimer = coyoteTimer = playerBehaviour.PlayerData.GetCoyoteTime;
            }
            if (coyoteTimer <= 0)
            {
                playerBehaviour.ChangeState(playerBehaviour.falling);
            }
        }
    }

    public override void FixedUpdate()
    {
        playerBehaviour.transform.rotation = playerBehaviour.RotateCharacter(playerBehaviour.moveDir) * AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, normal,
                                             playerBehaviour.PlayerData.GetSlopeCheckerLength, playerBehaviour.PlayerData.GetMaxRotationAngle);

        playerBehaviour.moveDir = Vector3.ProjectOnPlane(playerBehaviour.moveDir, normal).normalized;
        //Gravity
        playerBehaviour.rb.AddForce(-normal * InternalGravity, ForceMode.Acceleration);
        
        ApplyAcceleration(playerBehaviour.PlayerData.GetSpeedRelated.run.speed,playerBehaviour.PlayerData.GetSpeedRelated.run.accTotalTime);
        
        CreateSoundAlert();
    }
    
    public override void OnCTRL(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerBehaviour.ChangeState(playerBehaviour.walking);
        }
    }

    public override void OnWASD(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerBehaviour.ChangeState(playerBehaviour.idle);
        }
    }
    
    public override void OnShrink(InputAction.CallbackContext context)
    {
        if (context.performed && playerBehaviour.GetAbilities.HasFlag(AbilityData.Abilities.Shrink))
        {
            playerBehaviour.ChangeState(playerBehaviour.shrink);
        }
    }
    
    public override void OnDrawGizmos(PlayerBehaviour player)
    {
        if (gizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.transform.position, player.GetNormalPlayerData.GetAlertingRanges.run);
            Gizmos.DrawWireSphere(player.transform.position, player.GetShrinkPlayerData.GetAlertingRanges.run);
        }
    }
    
    public void FixCurrentAccelerationTime()
    {
        float currentSpeed = playerBehaviour.rb.velocity.magnitude;
        float maxSpeed = playerBehaviour.PlayerData.GetSpeedRelated.run.speed;
        float totalAccelerationTime = playerBehaviour.PlayerData.GetSpeedRelated.run.accTotalTime;
        playerBehaviour.accTime = CalculateAccelerationTimeFromSpeed(currentSpeed,maxSpeed,totalAccelerationTime);
    }
    
    protected override void CreateSoundAlert()
    {
        SoundAlertInfo soundAlertInfo = new SoundAlertInfo
        {
            soundRange = playerBehaviour.PlayerData.GetAlertingRanges.run,
            point = playerBehaviour.transform.position
        };
            
        SoundAlertCreation.CreateAlertPoint(soundAlertInfo,playerBehaviour.EnemyManagerPort);
    }
    
}
