using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gliding : State
{
    public Gliding(PlayerBehaviour playerBehaviour) : base(playerBehaviour) {}

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        if (ExitGlide())
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);
        }
    }

    public override void FixedUpdate()
    {
        playerBehaviour.rb.velocity = new Vector3(playerBehaviour.rb.velocity.x, playerBehaviour.PlayerData.GetGlideFallingSpeed, playerBehaviour.rb.velocity.z);
        ApplyCorrectiveAirForces();
        UpdateAirborneRotation2(playerBehaviour.rb, playerBehaviour.transform, ref playerBehaviour.currentVelocity, playerBehaviour.smoothTime);
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);
        }
    }
}
