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


    public override void Enter()
    {
        playerBehaviour.rb.velocity = Vector3.zero;
        
        if(playerBehaviour.moveInput != null )
        {
            //playerBehaviour.ChangeState(playerBehaviour.walkState);

            
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
        Debug.Log("IDLE");
    }
    public override void Exit()
    {

    }

    public override void OnCollision(Collision collision)
    {

    }

    public override void Update()
    {
        if (!CheckForGround())
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);
        }
    }

    public override void FixedUpdate()
    {

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

    public bool CheckForGround()
    {
        foreach (Transform t in playerBehaviour.rayCastPoints)
        {
            if (Physics.Raycast(t.position, Vector3.down, 0.1f))
            {
                return true;
            }
        }
        return false;
    }
}
