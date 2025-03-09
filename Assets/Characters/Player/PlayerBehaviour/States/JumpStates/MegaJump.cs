using System;
using System.Collections;
using System.Collections.Generic;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class MegaJump : JumpState
{
    [SerializeField] private MegaJumpsSoundInfos SoundInformations;
        
    private float chargeTimer;
    
    public override void Enter()
    {
        chargeTimer = playerBehaviour.PlayerData.GetChargeTime;
    }

    public override void Update()
    {
        chargeTimer -= Time.deltaTime;
    }

    public override void OnShift(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerBehaviour.ChangeJumpState(playerBehaviour.normalJump);
        }
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.canceled && chargeTimer <= 0)
        {
            float jumpForce = PhysicsCalculations.ForceToJumpCertainHeight(playerBehaviour.PlayerData.GetMegaJump.GetJumpHeight, 1, playerBehaviour.PlayerData.GetGravityMagnitudeUp);
            Jump(jumpForce, playerBehaviour.PlayerData.GetMegaJump.GetKeptMomentumPercentage);
            chargeTimer = playerBehaviour.PlayerData.GetChargeTime;
        }
        else if (context.canceled)
        {
            float jumpForce = PhysicsCalculations.ForceToJumpCertainHeight(playerBehaviour.PlayerData.GetNormalJump.GetJumpHeight, 1, playerBehaviour.PlayerData.GetGravityMagnitudeUp);
            Jump(jumpForce, playerBehaviour.PlayerData.GetNormalJump.GetKeptMomentumPercentage);
        }
    }
}

[Serializable]
public struct MegaJumpsSoundInfos
{
    public SoundInfo[] OnCharged;
    public SoundInfo[] OnJump;
}
