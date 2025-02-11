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


    public override void Enter()
    {
        Debug.Log("FALLING");
        playerBehaviour.ChangeJumpState(playerBehaviour.unableToJump);
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
        if(CheckForGround())
        {
            playerBehaviour.ChangeState(playerBehaviour.idle);
            playerBehaviour.ChangeJumpState(playerBehaviour.ableToJump);
        }


        targetRotation = Quaternion.FromToRotation(playerBehaviour.transform.up, Vector3.up) * playerBehaviour.transform.rotation;
        playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, targetRotation, time);

        time += 2 * Time.deltaTime;
    }

    public override void FixedUpdate()
    {
        ApplyCorrectiveAirForces();
        
        //Gravity
        playerBehaviour.rb.AddForce(Vector3.down * playerBehaviour.GetMovementData.GetGravityMagnitudeDown, ForceMode.Force);
        
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
        LayerMask layerToIgnore = 1 << 6;
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
