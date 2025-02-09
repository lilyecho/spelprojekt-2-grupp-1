using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargingJump : JumpState
{
    public ChargingJump(PlayerBehaviour playerBehaviour) : base(playerBehaviour) { }


    public float chargeTimer;

    public override void Enter()
    {
        chargeTimer = playerBehaviour.jumpChargeTime;
        Debug.Log("CHARGINGJUMP");
    }
    public override void Exit()
    {

    }

    public override void Update()
    {
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
            Jump(jumpForce);
        }
    }


}
