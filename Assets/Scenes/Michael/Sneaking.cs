using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sneaking : State
{
    public Sneaking(PlayerBehaviour playerBehaviour) : base(playerBehaviour)
    {

    }


    public override void Enter()
    {
        Debug.Log("SNEAKING");
        playerBehaviour.moveSpeed = playerBehaviour.sneakSpeed;
    }
    public override void Exit()
    {

    }

    public override void OnCollision(Collision collision)
    {

    }

    public override void Update()
    {
        playerBehaviour.RotateCharacter(playerBehaviour.moveDir);
    }

    public override void FixedUpdate()
    {
        playerBehaviour.rb.velocity = playerBehaviour.moveDir.normalized * playerBehaviour.moveSpeed;
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {

    }
    public override void OnShift(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerBehaviour.ChangeState(playerBehaviour.walking);
        }
    }
    public override void OnCTRL(InputAction.CallbackContext context)
    {

    }

    public override void OnWASD(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerBehaviour.ChangeState(playerBehaviour.idle);
        }
    }
    public override void OnMOUSE(InputAction.CallbackContext context)
    {

    }
}
