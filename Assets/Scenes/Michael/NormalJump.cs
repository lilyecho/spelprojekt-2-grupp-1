using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class NormalJump : JumpState
{
    public NormalJump(PlayerBehaviour playerBehaviour) : base(playerBehaviour) { }
    
    public override void Enter()
    {
        Debug.Log("Normal Jump State");
        if (playerBehaviour.intoJump)
        {
            float jumpForce = PhysicsCalculations.ForceToJumpCertainHeight(playerBehaviour.GetMovementData.GetNormalJump.GetJumpHeight, 1, playerBehaviour.GetMovementData.GetGravityMagnitudeUp);
            Jump(jumpForce, playerBehaviour.GetMovementData.GetNormalJump.GetKeptMomentumPercentage);
            playerBehaviour.intoJump = false;
        }
        //sneak isn't deactivated so change to megaJump-state 
        else if (playerBehaviour.movementMode == PlayerBehaviour.MovementMode.SNEAK)
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
        if (context.performed)
        {
            playerBehaviour.ChangeJumpState(playerBehaviour.megaJump);
        }
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float jumpForce = PhysicsCalculations.ForceToJumpCertainHeight(playerBehaviour.GetMovementData.GetNormalJump.GetJumpHeight, 1,  playerBehaviour.GetMovementData.GetGravityMagnitudeUp);
            Jump(jumpForce,playerBehaviour.GetMovementData.GetNormalJump.GetKeptMomentumPercentage);
        }
    }
    
    
    
    /*public override void Update()
    {
        TryChargeMegaJump();
    }

    private void TryChargeMegaJump()
    {
        if (!playerBehaviour.Abilities.HasFlag(AbilityData.Abilities.MegaJump)) return;

        chargeTimer -= Time.deltaTime;
        if (chargeTimer < 0)
        {
            playerBehaviour.ChangeJumpState(playerBehaviour.megaJump);
        }
    }*/

    
}
