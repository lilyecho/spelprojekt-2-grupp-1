using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

    public override void Update()
    {
        /*
        playerBehaviour.transform.rotation = Quaternion.Lerp(playerBehaviour.transform.rotation, 
                                                        UpdateAirborneRotation(playerBehaviour.moveInput, playerBehaviour.transform, playerBehaviour.rb), 
                                                        playerBehaviour.rotationSpeed * Time.deltaTime);
        */
        //float angle = UpdateAirborneRotation2(playerBehaviour.rb, playerBehaviour.transform, playerBehaviour.currentVelocity,playerBehaviour.smoothTime);
        //playerBehaviour.transform.rotation = Quaternion.Euler(playerBehaviour.transform.eulerAngles.x, angle, playerBehaviour.transform.eulerAngles.z);
        
        if (flagAbleToFall && playerBehaviour.rb.velocity.normalized.y <= 0)
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);
        }
    }

    public override void FixedUpdate()
    {
        UpdateAirborneRotation2(playerBehaviour.rb, playerBehaviour.transform, ref playerBehaviour.currentVelocity, playerBehaviour.smoothTime);
        
        ApplyCorrectiveAirForces();
        
        //Gravity
        playerBehaviour.rb.AddForce(Vector3.down * playerBehaviour.GetMovementData.GetGravityMagnitudeUp, ForceMode.Acceleration);

        flagAbleToFall = true;
        ChangeRotationToStandard();
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //playerBehaviour.ChangeState(playerBehaviour.gliding);
        }

    }
}
