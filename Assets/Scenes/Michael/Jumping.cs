using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : State
{
    public Jumping(PlayerBehaviour playerBehaviour) : base(playerBehaviour)
    {

    }

    //float pisstid = 0.1f;
    //float pisstidstimer;

    private bool test;

    [SerializeField] private Vector3 airActiveCorrectiveForces;
    
    public override void Enter()
    {
        Debug.Log("JUMPING");
        //pisstidstimer = pisstid;
        //playerBehaviour.ChangeJumpState(playerBehaviour.unableToJump);

    }
    public override void Exit()
    {
        test = false;
    }

    public override void OnCollision(Collision collision)
    {

    }

    public override void Update()
    {
        //Byta till falling vector y är mindre än 0
        if(test && playerBehaviour.rb.velocity.normalized.y <= 0)
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);
        }
        //pisstidstimer -= Time.deltaTime;
    }

    public override void FixedUpdate()
    {
        playerBehaviour.rb.AddForce(playerBehaviour.moveDir.normalized * playerBehaviour.GetMovementData.GetMidAirForces.GetAppliedMagnitude, ForceMode.Acceleration);
        
        //Gravity
        playerBehaviour.rb.AddForce(Vector3.down * playerBehaviour.GetMovementData.GetGravityMagnitudeUp, ForceMode.Acceleration);

        test = true;
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
