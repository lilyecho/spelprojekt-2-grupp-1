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
            playerBehaviour.rb.AddForce(Vector3.up * playerBehaviour.jumpForce * 1.5f, ForceMode.Impulse);
            playerBehaviour.ChangeState(playerBehaviour.jumping);
            playerBehaviour.ChangeJumpState(playerBehaviour.unableToJump);
        }
    }
}
