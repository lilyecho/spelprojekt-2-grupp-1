using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SceneHandling.SoundSystem.Scripts;

[Serializable]
public class Gliding : State
{
    
    #region AnimationParameters

    private int animationGlide = Animator.StringToHash("Gliding");

    #endregion
    // Audio parameters
    [SerializeField] private SoundInfos soundInfos;
    public override void Enter()
    {
        playerBehaviour.anim.SetBool(animationGlide, true);
        playerBehaviour.GetAudioPort.OnSoundInfos(soundInfos.onEnter);
    }

    public override void Exit()
    {
        playerBehaviour.anim.SetBool(animationGlide, false);
        playerBehaviour.GetAudioPort.OnSoundInfos(soundInfos.onExit);
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
    [Serializable]
    private struct SoundInfos
    {   
        public SoundInfo[] onEnter;
        public SoundInfo[] onExit;
    }
}
