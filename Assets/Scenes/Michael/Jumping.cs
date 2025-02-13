using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : State
{
    public Jumping(PlayerBehaviour playerBehaviour) : base(playerBehaviour)
    {

    }

    private bool flagAbleToFall;
    
    public override void Enter()
    {
        Debug.Log("JUMPING");
        //playerBehaviour.ChangeJumpState(playerBehaviour.unableToJump);

    }
    public override void Exit()
    {
        flagAbleToFall = false;
    }

    public override void OnCollision(Collision collision)
    {

    }

    public override void Update()
    {
        playerBehaviour.transform.rotation = Quaternion.Lerp(playerBehaviour.transform.rotation, 
                                                        UpdateAirborneRotation(playerBehaviour.moveInput, playerBehaviour.transform, playerBehaviour.rb), 
                                                        playerBehaviour.rotationSpeed * Time.deltaTime);

        if (flagAbleToFall && playerBehaviour.rb.velocity.normalized.y <= 0)
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);
        }
    }

    public override void FixedUpdate()
    {
        ApplyCorrectiveAirForces();
        
        //Gravity
        playerBehaviour.rb.AddForce(Vector3.down * playerBehaviour.GetMovementData.GetGravityMagnitudeUp, ForceMode.Force);

        flagAbleToFall = true;
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
        //airActiveCorrectiveForces = 
    }
    public override void OnMOUSE(InputAction.CallbackContext context)
    {

    }


    
}
