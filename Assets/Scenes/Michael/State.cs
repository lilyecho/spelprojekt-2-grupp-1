using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class State
{


    public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }
    public virtual void OnCollision(Collision collision)
    {

    }

    public virtual void Update()
    {

    }
    public virtual void FixedUpdate()
    {

    }

    public virtual void OnSpaceBar(InputAction.CallbackContext context)
    {

    }
    public virtual void OnShift(InputAction.CallbackContext context)
    {

    }
    public virtual void OnCTRL(InputAction.CallbackContext context)
    {

    }

    public virtual void OnWASD(InputAction.CallbackContext context)
    {

    }
    public virtual void OnMOUSE(InputAction.CallbackContext context)
    {

    }


    protected PlayerBehaviour playerBehaviour;


    public State(PlayerBehaviour playerBehaviour)
    {
        this.playerBehaviour = playerBehaviour;
    }

}
