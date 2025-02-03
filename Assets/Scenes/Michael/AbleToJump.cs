using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbleToJump : JumpState
{
    public AbleToJump(PlayerBehaviour playerBehaviour) : base(playerBehaviour) { }

    

    public override void Enter()
    {
        
        Debug.Log("ABLETOJUMP");
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
        if (context.performed)
        {
            playerBehaviour.ChangeJumpState(playerBehaviour.chargingJump);

        }
    }

    
}
