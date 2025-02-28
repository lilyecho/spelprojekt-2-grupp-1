using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Idle : State
{
    public Idle(PlayerBehaviour playerBehaviour) : base(playerBehaviour)
    {

    }
    float time = 0f;
    Vector3 normal;

    public override void Enter()
    {
        if(playerBehaviour.moveInput == Vector2.zero)
        {
            playerBehaviour.rb.velocity = Vector3.zero;
        }
        
        if (playerBehaviour.moveInput != Vector2.zero)
        {
            switch (playerBehaviour.movementMode)
            {
                case PlayerBehaviour.MovementMode.SNEAK:
                    playerBehaviour.ChangeState(playerBehaviour.sneaking);
                    break;
                case PlayerBehaviour.MovementMode.RUN:
                    playerBehaviour.ChangeState(playerBehaviour.running);
                    break;
                case PlayerBehaviour.MovementMode.WALK:
                    playerBehaviour.ChangeState(playerBehaviour.walking);
                    break;
            }
        }
        else
        {
            playerBehaviour.anim.Play("Astrid_Idle_Anim");
        }
        Debug.Log("IDLE");
    }
    public override void Exit()
    {
        //StopDeAcceleration();
    }

    public override void OnCollision(Collision collision)
    {

    }

    public override void Update()
    {
        playerBehaviour.RotateCharacter(playerBehaviour.moveDir);
        normal = GetSurfaceNormal(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength);
        //playerBehaviour.transform.rotation = AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, time, Vector3.up);
        playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, normal,
                                                                playerBehaviour.GetMovementData.GetSlopeCheckerLength, playerBehaviour.GetMovementData.GetMaxRotationAngle), time);
        time = time + Time.deltaTime;
        if (!CheckForGround(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength * 1.5f))
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);
        }
    }

    public override void FixedUpdate()
    {
        playerBehaviour.rb.AddForce(-normal * 9.81f, ForceMode.Acceleration);
        playerBehaviour.moveDir = Vector3.ProjectOnPlane(playerBehaviour.moveDir, normal).normalized;
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {

    }
    public override void OnShift(InputAction.CallbackContext context)
    {
    }
    public override void OnCTRL(InputAction.CallbackContext context)
    {

    }

    public override void OnWASD(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //playerBehaviour.ChangeState(playerBehaviour.walkState);


            switch (playerBehaviour.movementMode)
            {
                case PlayerBehaviour.MovementMode.SNEAK:
                    playerBehaviour.ChangeState(playerBehaviour.sneaking);
                    break;
                case PlayerBehaviour.MovementMode.RUN:
                    playerBehaviour.ChangeState(playerBehaviour.running);
                    break;
                case PlayerBehaviour.MovementMode.WALK:
                    playerBehaviour.ChangeState(playerBehaviour.walking);
                    break;
            }
        }
    }
    public override void OnMOUSE(InputAction.CallbackContext context)
    {

    }
    
    /*/// <summary>
    /// Only handle from currentSpeed to zero in idle
    /// </summary>
    private void DoDeAcceleration()
    {
        playerBehaviour.StartCoroutine(DeAccelerate());
    }
    private void StopDeAcceleration()
    {
        playerBehaviour.StopCoroutine(DeAccelerate());
    }

    private IEnumerator DeAccelerate()
    {
        while (playerBehaviour.rb.velocity.magnitude > 0)
        {
            yield return new WaitForSeconds(playerBehaviour.GetMovementData.GetDeAcceleration);
            break;
        }
        playerBehaviour.rb.velocity = Vector3.zero;
    }*/
}
