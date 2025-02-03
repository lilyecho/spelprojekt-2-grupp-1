using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Walking : State
{
    public Walking(PlayerBehaviour playerBehaviour) : base(playerBehaviour)
    {

    }


    public override void Enter()
    {
        Debug.Log("WALKING");
        playerBehaviour.moveSpeed = playerBehaviour.walkSpeed;
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

        if (!CheckForGround())
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);
        }
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
        if (context.performed)
        {
            playerBehaviour.ChangeState(playerBehaviour.sneaking);
        }
    }
    public override void OnCTRL(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            playerBehaviour.ChangeState(playerBehaviour.running);
        }
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

    public bool CheckForGround()
    {
        foreach (Transform t in playerBehaviour.rayCastPoints)
        {
            if (Physics.Raycast(t.position, Vector3.down, 0.1f))
            {
                return true;
            }
        }
        return false;
    }
}
