using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MegaJump : JumpState
{
    public MegaJump(PlayerBehaviour playerBehaviour) : base(playerBehaviour) { }

    private float chargeTimer;
    
    public override void Enter()
    {
        Debug.Log("Mega Jump state");
        chargeTimer = playerBehaviour.GetPlayerData.GetChargeTime;
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
            float jumpForce = PhysicsCalculations.ForceToJumpCertainHeight(playerBehaviour.GetPlayerData.GetMegaJump.GetJumpHeight, 1, playerBehaviour.GetPlayerData.GetGravityMagnitudeUp);
            Jump(jumpForce, playerBehaviour.GetPlayerData.GetMegaJump.GetKeptMomentumPercentage);
            chargeTimer = playerBehaviour.GetPlayerData.GetChargeTime;
        }
        else if (context.canceled)
        {
            float jumpForce = PhysicsCalculations.ForceToJumpCertainHeight(playerBehaviour.GetPlayerData.GetNormalJump.GetJumpHeight, 1, playerBehaviour.GetPlayerData.GetGravityMagnitudeUp);
            Jump(jumpForce, playerBehaviour.GetPlayerData.GetNormalJump.GetKeptMomentumPercentage);
        }
    }

    
    
}
