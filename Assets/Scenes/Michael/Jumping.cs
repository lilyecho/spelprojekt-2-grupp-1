using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : State
{
    public Jumping(PlayerBehaviour playerBehaviour) : base(playerBehaviour)
    {

    }

    float pisstid = 0.1f;
    float pisstidstimer;

    public override void Enter()
    {
        Debug.Log("JUMPING");
        pisstidstimer = pisstid;
        //playerBehaviour.ChangeJumpState(playerBehaviour.unableToJump);

    }
    public override void Exit()
    {

    }

    public override void OnCollision(Collision collision)
    {

    }

    public override void Update()
    {
        if(playerBehaviour.rb.velocity.y < 0 && pisstidstimer <= 0)
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);

        }
        pisstidstimer -= Time.deltaTime;
    }

    public override void FixedUpdate()
    {
        playerBehaviour.rb.AddForce(playerBehaviour.moveDir.normalized * playerBehaviour.moveSpeed, ForceMode.Acceleration);

        playerBehaviour.rb.velocity += new Vector3(0, -0.8f, 0);
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


    
}
