using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class NormalJump : JumpState
{
    public override void Enter()
    {
        if (playerBehaviour.intoJump)
        {
            float jumpForce = PhysicsCalculations.ForceToJumpCertainHeight(playerBehaviour.PlayerData.GetNormalJump.GetJumpHeight, 1, playerBehaviour.PlayerData.GetGravityMagnitudeUp);
            Jump(jumpForce, playerBehaviour.PlayerData.GetNormalJump.GetKeptMomentumPercentage, playerBehaviour.jumpParticles);
            playerBehaviour.intoJump = false;
        }
        //sneak isn't deactivated so change to megaJump-state 
        else if (playerBehaviour.GetAbilities.HasFlag(AbilityData.Abilities.MegaJump) && playerBehaviour.movementMode == PlayerBehaviour.MovementMode.SNEAK)
        {
            playerBehaviour.ChangeJumpState(playerBehaviour.megaJump);
        }
    }
    public override void Exit()
    {

    }

    public override void Update()
    {
        
    }
    public override void FixedUpdate()
    {

    }

    public override void OnShift(InputAction.CallbackContext context)
    {
        if (context.performed && playerBehaviour.movementMode == PlayerBehaviour.MovementMode.SNEAK)
        {
            if (!playerBehaviour.GetAbilities.HasFlag(AbilityData.Abilities.MegaJump)) return;
            playerBehaviour.ChangeJumpState(playerBehaviour.megaJump);
        }
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float jumpForce = PhysicsCalculations.ForceToJumpCertainHeight(playerBehaviour.PlayerData.GetNormalJump.GetJumpHeight, 1,  playerBehaviour.PlayerData.GetGravityMagnitudeUp);
            Jump(jumpForce,playerBehaviour.PlayerData.GetNormalJump.GetKeptMomentumPercentage, playerBehaviour.jumpParticles);
        }
    }
    
    [Serializable]
    private struct SoundInfos
    {
        public EventInfo charged;
        public EventInfo test2;
        public EventInfo test3;
    }
}


