using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargingJump : JumpState
{
    public ChargingJump(PlayerBehaviour playerBehaviour) : base(playerBehaviour) { }


    public float chargeTime = 0.5f;
    public float chargeTimer;

    public override void Enter()
    {
        chargeTimer = chargeTime;
        Debug.Log("CHARGINGJUMP");
    }
    public override void Exit()
    {

    }

    public override void Update()
    {
        chargeTimer -= Time.deltaTime;
        if (chargeTimer < 0)
        {
            playerBehaviour.ChangeJumpState(playerBehaviour.jumpCharged);
        }
    }
    public override void FixedUpdate()
    {

    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerBehaviour.rb.AddForce(Vector3.up * playerBehaviour.jumpForce, ForceMode.Impulse);
            playerBehaviour.ChangeState(playerBehaviour.jumping);
            playerBehaviour.ChangeJumpState(playerBehaviour.unableToJump);
        }
    }


}
