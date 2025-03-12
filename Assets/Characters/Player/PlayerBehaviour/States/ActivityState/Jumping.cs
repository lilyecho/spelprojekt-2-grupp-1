using System;
using System.Collections;
using System.Collections.Generic;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

[Serializable]
public class Jumping : State
{
    private bool flagAbleToFall;
    [SerializeField] private SoundInfos soundInfos;
    
    #region AnimationParameters

    private int animationSuperJumpActivate = Animator.StringToHash("SuperJumpActivate");
    private int animationGrounded = Animator.StringToHash("Grounded");
    private int animationJump = Animator.StringToHash("Jump");

    #endregion
    
    public override void Enter()
    {
        playerBehaviour.anim.SetBool(animationSuperJumpActivate, false);
        playerBehaviour.GetAudioPort.OnSoundInfos(soundInfos.onEnter);
        playerBehaviour.anim.SetBool(animationGrounded, false);
    }
    public override void Exit()
    {
        playerBehaviour.anim.SetBool(animationJump, false);
        flagAbleToFall = false;
    }

    public override void Update()
    {
        if (flagAbleToFall && playerBehaviour.rb.velocity.normalized.y <= 0)
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);
        }
    }

    public override void FixedUpdate()
    {
        UpdateAirborneRotation2(playerBehaviour.rb, playerBehaviour.transform, ref playerBehaviour.currentVelocity, playerBehaviour.smoothTime);
        
        ApplyCorrectiveAirForces();
        
        //Gravity
        playerBehaviour.rb.AddForce(Vector3.down * playerBehaviour.PlayerData.GetGravityMagnitudeUp, ForceMode.Acceleration);

        flagAbleToFall = true;
        ChangeRotationToStandard();
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (Glide())
            {
                playerBehaviour.ChangeState(playerBehaviour.gliding);
            }
        }
    }

    [Serializable]
    private struct SoundInfos
    {   
        public SoundInfo[] onEnter;
    }
}
