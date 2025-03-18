using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public abstract class JumpState
{
    protected PlayerBehaviour playerBehaviour;

    public virtual void Awake(PlayerBehaviour player)
    {
        playerBehaviour = player;
        
    }
    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Update() {}
    public virtual void FixedUpdate() {}
    public virtual void OnSpaceBar(InputAction.CallbackContext context) {} 
    public virtual void OnShift(InputAction.CallbackContext context) {}
    public virtual void OnValidate() {}
    
    

    protected void Jump(float jumpForce, float momentumKeptPercentage, ParticleSystem part)
    {
        //Debug.Log(playerBehaviour.rb.velocity);
        playerBehaviour.rb.velocity *= momentumKeptPercentage; 
        playerBehaviour.rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerBehaviour.JumpParticles(part);
        playerBehaviour.anim.SetBool(Animator.StringToHash("Jump"), true);
        playerBehaviour.ChangeState(playerBehaviour.jumping);
        playerBehaviour.ChangeJumpState(playerBehaviour.unableToJump);
    }
}
