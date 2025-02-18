using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class Falling : State
{
    public Falling(PlayerBehaviour playerBehaviour) : base(playerBehaviour)
    {

    }

    /*
    bool intoJump = false;
    bool intoChargingJump = false;
    */

    float jumpBufferTimer;


    public override void Enter()
    {
        Debug.Log("FALLING");
        playerBehaviour.ChangeJumpState(playerBehaviour.unableToJump);
        jumpBufferTimer = playerBehaviour.GetMovementData.GetJumpBufferDuration;
        time = 0;
    }
    public override void Exit()
    {

    }

    public override void OnCollision(Collision collision)
    {

    }

    Quaternion targetRotation;
    float time;

    public override void Update()
    {
        /*
        playerBehaviour.transform.rotation = Quaternion.Lerp(playerBehaviour.transform.rotation,
                                                        UpdateAirborneRotation(playerBehaviour.moveInput, playerBehaviour.transform, playerBehaviour.rb),
                                                        playerBehaviour.rotationSpeed * Time.deltaTime);
        */
        //float angle = UpdateAirborneRotation2(playerBehaviour.rb, playerBehaviour.transform, playerBehaviour.currentVelocity, playerBehaviour.smoothTime);
        //playerBehaviour.transform.rotation = Quaternion.Euler(playerBehaviour.transform.eulerAngles.x, angle, playerBehaviour.transform.eulerAngles.z);

        UpdateAirborneRotation2(playerBehaviour.rb, playerBehaviour.transform, ref playerBehaviour.currentVelocity, playerBehaviour.smoothTime);

        if (CheckForGround())
        {
            
            playerBehaviour.ChangeState(playerBehaviour.idle);

            if(playerBehaviour.intoChargingJump)
            {
                playerBehaviour.ChangeJumpState(playerBehaviour.chargingJump);
                playerBehaviour.intoChargingJump = false;
            }
            else
            {
                playerBehaviour.ChangeJumpState(playerBehaviour.ableToJump);
            }
            
        }

        /*
        targetRotation = Quaternion.FromToRotation(playerBehaviour.transform.up, Vector3.up) * playerBehaviour.transform.rotation;
        playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, targetRotation, time);
        */
        

        time += 2 * Time.deltaTime;




        if(playerBehaviour.intoJump || playerBehaviour.intoChargingJump)
        {
            jumpBufferTimer -= Time.deltaTime;
        }
        if(jumpBufferTimer <= 0 )
        {
            playerBehaviour.intoJump = false;
            playerBehaviour.intoChargingJump = false;
        }
    }

    public override void FixedUpdate()
    {
        ApplyCorrectiveAirForces();
        
        //Gravity
        playerBehaviour.rb.AddForce(Vector3.down * playerBehaviour.GetMovementData.GetGravityMagnitudeDown, ForceMode.Acceleration);
        
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //playerBehaviour.ChangeState(playerBehaviour.gliding);

            playerBehaviour.intoChargingJump = true;
            jumpBufferTimer = playerBehaviour.GetMovementData.GetJumpBufferDuration;
        }

        if (context.canceled)
        {
            if (playerBehaviour.intoChargingJump)
            {
                playerBehaviour.intoJump = true;
                playerBehaviour.intoChargingJump = false;
            }
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
        LayerMask layerToIgnore = 1 << 8;
        RaycastHit hit;
        foreach (Transform t in playerBehaviour.rayCastPoints)
        {
            /*
            if (Physics.Raycast(t.position, Vector3.down, playerBehaviour.rayCastLength, ~layerToIgnore))
            {
                return true;
            }
            */
            if (Physics.Raycast(t.position, Vector3.down, out hit, playerBehaviour.rayCastLength, ~layerToIgnore))
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                //Debug.Log(angle);
                if (angle < 30f)
                {
                    return true;
                }

            }
        }
        return false;
    }

    
}
