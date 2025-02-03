using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnableToJump : JumpState
{
    public UnableToJump(PlayerBehaviour playerBehaviour) : base(playerBehaviour) { }


    public override void Enter()
    {
        Debug.Log("UNABLETOJUMP");
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

    }
}
