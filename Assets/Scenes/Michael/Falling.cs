using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Falling : State
{
    public Falling(PlayerBehaviour playerBehaviour) : base(playerBehaviour)
    {

    }


    public override void Enter()
    {
        Debug.Log("FALLING");
        

    }
    public override void Exit()
    {

    }

    public override void OnCollision(Collision collision)
    {

    }

    public override void Update()
    {
        if(CheckForGround())
        {
            playerBehaviour.ChangeState(playerBehaviour.idle);
            playerBehaviour.ChangeJumpState(playerBehaviour.ableToJump);
        }
    }

    public override void FixedUpdate()
    {
        playerBehaviour.rb.AddForce(playerBehaviour.moveDir.normalized * playerBehaviour.moveSpeed, ForceMode.Acceleration);
        
        //Gravity
        playerBehaviour.rb.AddForce(Vector3.down * playerBehaviour.GetMovementData.GetGravityMagnitudeDown, ForceMode.Acceleration);
        
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //playerBehaviour.ChangeState(playerBehaviour.gliding);
        }

    }
    public override void OnShift(InputAction.CallbackContext context)
    {

    }
    public override void OnCTRL(InputAction.CallbackContext context)
    {

    }

    public override void OnWASD(InputAction.CallbackContext context)
    {

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
