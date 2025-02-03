using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : State
{
    public Jumping(PlayerBehaviour playerBehaviour) : base(playerBehaviour)
    {

    }


    public override void Enter()
    {
        Debug.Log("JUMPING");
        playerBehaviour.ChangeJumpState(playerBehaviour.unableToJump);

    }
    public override void Exit()
    {

    }

    public override void OnCollision(Collision collision)
    {
        
        /*
        if (collision.gameObject.CompareTag("Tree"))
        {
            playerBehaviour.wallJumpDir = new Vector3(playerBehaviour.gameObject.transform.position.x - collision.gameObject.transform.position.x,
                                                      0,
                                                      playerBehaviour.gameObject.transform.position.z - collision.gameObject.transform.position.z).normalized;
            playerBehaviour.ChangeState(playerBehaviour.onTree);
        }
        */
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        playerBehaviour.rb.AddForce(playerBehaviour.moveDir.normalized * playerBehaviour.moveSpeed, ForceMode.Acceleration);

        playerBehaviour.rb.velocity += new Vector3(0, -0.5f, 0);
        /*
        if (CheckForGround() && playerBehaviour.rb.velocity.y < -0.1f)
        {
            playerBehaviour.ChangeState(playerBehaviour.idle);
            playerBehaviour.ChangeJumpState(playerBehaviour.ableToJump);
        }
        */
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
            if (Physics.Raycast(t.position, Vector3.down, 0.2f))
            {
                return true;
            }
        }
        return false;
    }
}
