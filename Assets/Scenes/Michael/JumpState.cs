using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class JumpState
{


    public virtual void Enter()
    {

    }
    public virtual void Exit()
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
    

    protected PlayerBehaviour playerBehaviour;


    public JumpState(PlayerBehaviour playerBehaviour)
    {
        this.playerBehaviour = playerBehaviour;
    }

}
