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
        //jumpBufferTimer = playerBehaviour.GetMovementData.GetJumpBufferDuration;
    }
    public override void Exit()
    {

    }

    public override void OnCollision(Collision collision)
    {

    }

    Quaternion targetRotation;

    public override void Update()
    {
        /*
        playerBehaviour.transform.rotation = Quaternion.Lerp(playerBehaviour.transform.rotation,
                                                        UpdateAirborneRotation(playerBehaviour.moveInput, playerBehaviour.transform, playerBehaviour.rb),
                                                        playerBehaviour.rotationSpeed * Time.deltaTime);
        */
        //float angle = UpdateAirborneRotation2(playerBehaviour.rb, playerBehaviour.transform, playerBehaviour.currentVelocity, playerBehaviour.smoothTime);
        //playerBehaviour.transform.rotation = Quaternion.Euler(playerBehaviour.transform.eulerAngles.x, angle, playerBehaviour.transform.eulerAngles.z);
        
        if (CheckForGround())
        {
            playerBehaviour.ChangeState(playerBehaviour.idle);
            playerBehaviour.ChangeJumpState(playerBehaviour.normalJump);
        }

        /*
        targetRotation = Quaternion.FromToRotation(playerBehaviour.transform.up, Vector3.up) * playerBehaviour.transform.rotation;
        playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, targetRotation, time);
        */
        
        if(playerBehaviour.intoJump)
        {
            jumpBufferTimer -= Time.deltaTime;
        }
        if(jumpBufferTimer <= 0 )
        {
            playerBehaviour.intoJump = false;
        }
    }

    public override void FixedUpdate()
    {
        UpdateAirborneRotation2(playerBehaviour.rb, playerBehaviour.transform, ref playerBehaviour.currentVelocity, playerBehaviour.smoothTime);
        
        ApplyCorrectiveAirForces();
        
        //Gravity
        playerBehaviour.rb.AddForce(Vector3.down * playerBehaviour.GetMovementData.GetGravityMagnitudeDown, ForceMode.Acceleration);
        
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpBufferTimer = playerBehaviour.GetMovementData.GetJumpBufferDuration;
        }

        if (context.canceled)
        {
            playerBehaviour.intoJump = true;
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
        LayerMask layerToIgnore = (1 << 8) | (1 << 2);
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
