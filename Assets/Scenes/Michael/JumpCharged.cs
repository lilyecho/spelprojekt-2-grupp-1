using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpCharged : JumpState
{
    public JumpCharged(PlayerBehaviour playerBehaviour) : base(playerBehaviour) { }


    public override void Enter()
    {
        Debug.Log("JUMPCHARGED");
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

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            float jumpForce = PhysicsCalculations.ForceToJumpCertainHeight(playerBehaviour.GetMovementData.GetMegaJump.GetJumpHeight, 1, playerBehaviour.GetMovementData.GetGravityMagnitudeUp);
            Jump(jumpForce);
        }
    }

    
    
}
