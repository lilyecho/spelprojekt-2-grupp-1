using System;
using System.Collections;
using System.Collections.Generic;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class MegaJump : JumpState
{
    [SerializeField] private JumpsSoundInfos SoundInformations;
        
    private float chargeTimer;

    #region AnimationParameters

    private int animationSuperJumpActivate = Animator.StringToHash("SuperJumpActivate");
    private int animationSuperJumpRelease = Animator.StringToHash("SuperJumpRelease");

    #endregion
    
    public override void Enter()
    {
        chargeTimer = playerBehaviour.PlayerData.GetChargeTime;
        playerBehaviour.anim.SetBool(animationSuperJumpActivate, true);
        playerBehaviour.GetAudioPort.OnSoundInfos(SoundInformations.onEnter);
        playerBehaviour.chargingJumpParticles.gameObject.SetActive(true);
    }

    public override void Update()
    {
        chargeTimer -= Time.deltaTime;
    }

    public override void OnShift(InputAction.CallbackContext context)
    {
        if (context.canceled || playerBehaviour.movementMode != PlayerBehaviour.MovementMode.SNEAK)
        {
            playerBehaviour.anim.SetBool(animationSuperJumpActivate, false);
            playerBehaviour.ChangeJumpState(playerBehaviour.normalJump);
        }
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.performed && chargeTimer <= 0)
        {
            playerBehaviour.anim.SetBool(animationSuperJumpRelease, true);
            playerBehaviour.GetAudioPort.OnSoundInfos(SoundInformations.onMegaJump);
            
            float jumpForce = PhysicsCalculations.ForceToJumpCertainHeight(playerBehaviour.PlayerData.GetMegaJump.GetJumpHeight, 1, playerBehaviour.PlayerData.GetGravityMagnitudeUp);
            Jump(jumpForce, playerBehaviour.PlayerData.GetMegaJump.GetKeptMomentumPercentage, playerBehaviour.megaJumpParticles);
            chargeTimer = playerBehaviour.PlayerData.GetChargeTime;
        }
        else if (context.performed)
        {
            playerBehaviour.anim.SetBool(animationSuperJumpActivate, false);
            playerBehaviour.GetAudioPort.OnSoundInfos(SoundInformations.onNormalJump);
            
            float jumpForce = PhysicsCalculations.ForceToJumpCertainHeight(playerBehaviour.PlayerData.GetNormalJump.GetJumpHeight, 1, playerBehaviour.PlayerData.GetGravityMagnitudeUp);
            Jump(jumpForce, playerBehaviour.PlayerData.GetNormalJump.GetKeptMomentumPercentage, playerBehaviour.jumpParticles);
        }
    }
    
    [Serializable]
    private struct JumpsSoundInfos
    {
        public SoundInfo[] onEnter;
        public SoundInfo[] onMegaJump;
        public SoundInfo[] onNormalJump;
    }

    public override void Exit()
    {
        playerBehaviour.chargingJumpParticles.gameObject.SetActive(false);
    }
}


