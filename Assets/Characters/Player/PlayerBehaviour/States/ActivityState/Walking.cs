using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Walking : State, IAcceleration
{
    [SerializeField] private bool gizmos;
    
    private float timeStep = .5f;
    private float currentTime = 0;
    
    float time = 0f;
    Vector3 normal;

    bool coyote = false;
    float coyoteTimer;

    [SerializeField] private SoundInfo[] soundInfos;
    
    public override void Enter()
    {
        //OnEnterChangeGlobalActivityParameter(playerBehaviour.GetParameterData.GetCatSneak, (int)CharacterActivity.Walk);
        GameManager.instance.PlayBellNormal();
        coyoteTimer = playerBehaviour.PlayerData.GetCoyoteTime;

        FixCurrentAccelerationTime();
    }

    public override void Update()
    {

        normal = GetSurfaceNormal(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength * 2);
        
        time = time + Time.deltaTime;

        if (!coyote && !CheckForGround(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength * 1.5f))
        {
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
        playerBehaviour.transform.rotation = playerBehaviour.RotateCharacter(playerBehaviour.moveDir) * AlignToSlope(
            playerBehaviour.rayCastPoints, playerBehaviour.transform, normal,
            playerBehaviour.PlayerData.GetSlopeCheckerLength, playerBehaviour.PlayerData.GetMaxRotationAngle);

        playerBehaviour.moveDir = Vector3.ProjectOnPlane(playerBehaviour.moveDir, normal).normalized;
        //Gravity
        playerBehaviour.rb.AddForce(-normal * InternalGravity, ForceMode.Acceleration);

        ApplyAcceleration(playerBehaviour.PlayerData.GetSpeedRelated.walk.speed,
            playerBehaviour.PlayerData.GetSpeedRelated.walk.accTotalTime);
        
        CreateSoundAlert(playerBehaviour.transform.position, playerBehaviour.PlayerData.GetAlertingRanges.walk);
    }

    public override void OnShift(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerBehaviour.ChangeState(playerBehaviour.sneaking);
        }
    }

    public override void OnCTRL(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerBehaviour.ChangeState(playerBehaviour.running);
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.transform.position, player.GetNormalPlayerData.GetAlertingRanges.walk);
            Gizmos.DrawWireSphere(player.transform.position, player.GetShrinkPlayerData.GetAlertingRanges.walk);
        }
    }

    public void FixCurrentAccelerationTime()
    {
        float currentSpeed = playerBehaviour.rb.velocity.magnitude;
        float maxSpeed = playerBehaviour.PlayerData.GetSpeedRelated.walk.speed;
        float totalAccelerationTime = playerBehaviour.PlayerData.GetSpeedRelated.walk.accTotalTime;
        playerBehaviour.accTime = CalculateAccelerationTimeFromSpeed(currentSpeed, maxSpeed, totalAccelerationTime);
    }
}
