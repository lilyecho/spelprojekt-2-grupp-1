using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargingJump : JumpState
{
    public ChargingJump(PlayerBehaviour playerBehaviour) : base(playerBehaviour) { }


    public float chargeTimer;

    public override void Enter()
    {
        chargeTimer = playerBehaviour.GetMovementData.GetChargeTime;
        Debug.Log("CHARGINGJUMP");
    }
    public override void Exit()
    {
        
    }


    private void ActivateSound()
    {
        EventReference eventRef = playerBehaviour.GetAudioData.GetAudioJump;
        playerBehaviour.GetAudioPort.OnCreate(playerBehaviour.GetAudioData.GetAudioJump);
    }
    
    public override void Update()
    {
        TryChargeMegaJump();
    }

    private void TryChargeMegaJump()
    {
        if (!playerBehaviour.Abilities.HasFlag(AbilityData.Abilities.MegaJump)) return;
        
        chargeTimer -= Time.deltaTime;
        if (chargeTimer < 0)
        {
            playerBehaviour.ChangeJumpState(playerBehaviour.jumpCharged);
        }
    }
    
    public override void FixedUpdate()
    {

    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            float jumpForce = PhysicsCalculations.ForceToJumpCertainHeight(playerBehaviour.GetMovementData.GetNormalJump.GetJumpHeight, 1,  playerBehaviour.GetMovementData.GetGravityMagnitudeUp);
            Jump(jumpForce,playerBehaviour.GetMovementData.GetNormalJump.GetKeptMomentumPercentage);
        }
    }


}
