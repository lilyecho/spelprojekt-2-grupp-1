using System;
using System.Collections;
using System.Collections.Generic;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[Serializable]
public class MegaJump : JumpState
{
    [SerializeField] private ParticleSystem chargingJumpParticles;
    [SerializeField] private ParticleSystem chargedJumpParticle;
    [SerializeField] private JumpsSoundInfos SoundInformations;

    private bool doneChargedEffect;
    private float chargeTimer;

    #region AnimationParameters

    private int animationSuperJumpActivate = Animator.StringToHash("SuperJumpActivate");
    private int animationSuperJumpRelease = Animator.StringToHash("SuperJumpRelease");

    #endregion
    
    public override void Enter()
    {
        chargeTimer = playerBehaviour.PlayerData.GetChargeTime;
        doneChargedEffect = false;
        
        playerBehaviour.anim.SetBool(animationSuperJumpActivate, true);
        playerBehaviour.GetAudioPort.OnSoundInfos(SoundInformations.onChargedMegaJump);
        chargingJumpParticles.gameObject.SetActive(true);
        
    }

    public override void Update()
    {
        chargeTimer -= Time.deltaTime;
        
        //Particles for when it is done charging
        if (!doneChargedEffect && chargeTimer <= 0)
        {
            chargedJumpParticle.Play();
            chargingJumpParticles.gameObject.SetActive(false);
            doneChargedEffect = true;
        }
    }

    public override void OnShift(InputAction.CallbackContext context)
    {
        if (context.canceled || playerBehaviour.movementMode != PlayerBehaviour.MovementMode.SNEAK)
        {
            playerBehaviour.GetAudioPort.OnSoundInfos(SoundInformations.onNoJump);
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
        [FormerlySerializedAs("onEnter")] public SoundInfo[] onChargedMegaJump;
        public SoundInfo[] onMegaJump;
        public SoundInfo[] onNormalJump;
        public SoundInfo[] onNoJump;
    }

    public override void Exit()
    {
        chargingJumpParticles.gameObject.SetActive(false);
    }
}


