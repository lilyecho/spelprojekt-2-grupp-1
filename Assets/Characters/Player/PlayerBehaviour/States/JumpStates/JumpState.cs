using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
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

    public virtual void OnShift(InputAction.CallbackContext context)
    {
        
    }
    

    protected PlayerBehaviour playerBehaviour;


    public JumpState(PlayerBehaviour playerBehaviour)
    {
        this.playerBehaviour = playerBehaviour;
    }

    protected void Jump(float jumpForce, float momentumKeptPercentage)
    {
        //Debug.Log(playerBehaviour.rb.velocity);
        playerBehaviour.rb.velocity *= momentumKeptPercentage; 
        playerBehaviour.rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerBehaviour.JumpParticles();
        playerBehaviour.ChangeState(playerBehaviour.jumping);
        playerBehaviour.ChangeJumpState(playerBehaviour.unableToJump);
    }
    
    protected void ActivateJumpSound()
    {
        EventReference eventRef = playerBehaviour.GetAudioData.GetAudioJump;
        playerBehaviour.GetAudioPort.OnCreate(playerBehaviour.GetAudioData.GetAudioJump);
    }
}
