using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Running : State, IAcceleration
{
    public Running(PlayerBehaviour playerBehaviour) : base(playerBehaviour)
    {

    }
    float time = 0f;
    Vector3 normal;

    public override void Enter()
    {
        Debug.Log("RUNNING");
        //playerBehaviour.moveSpeed = playerBehaviour.GetMovementData.GetSpeedRelated.run.speed;
        
        FixCurrentAccelerationTime();
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
        normal = GetSurfaceNormal(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength * 2);
        //playerBehaviour.transform.rotation = AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, time, Vector3.up);
        playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, normal), time);
        time = time + Time.deltaTime;
        if (!CheckForGround(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength * 1.5f))
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);
        }
    }

    public override void FixedUpdate()
    {
        //playerBehaviour.rb.AddForce(-normal * 9.81f, ForceMode.Acceleration);
        playerBehaviour.moveDir = Vector3.ProjectOnPlane(playerBehaviour.moveDir, normal).normalized;
        
        ApplyAcceleration(playerBehaviour.GetMovementData.GetSpeedRelated.run.speed,playerBehaviour.GetMovementData.GetSpeedRelated.run.accTotalTime);
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {

    }
    public override void OnShift(InputAction.CallbackContext context)
    {

    }
    public override void OnCTRL(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerBehaviour.ChangeState(playerBehaviour.walking);
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
    
    public void FixCurrentAccelerationTime()
    {
        float currentSpeed = playerBehaviour.rb.velocity.magnitude;
        float maxSpeed = playerBehaviour.GetMovementData.GetSpeedRelated.run.speed;
        float totalAccelerationTime = playerBehaviour.GetMovementData.GetSpeedRelated.run.accTotalTime;
        playerBehaviour.accTime = CalculateAccelerationTimeFromSpeed(currentSpeed,maxSpeed,totalAccelerationTime);
    }
    /*
    public bool CheckForGround()
    {
        foreach (Transform t in playerBehaviour.rayCastPoints)
        {
            if (Physics.Raycast(t.position, Vector3.down, playerBehaviour.rayCastLength))
            {
                return true;
            }
        }
        return false;
    }
    */
    
}
