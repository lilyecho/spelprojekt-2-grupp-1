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
    [SerializeField] private bool gizmos;
    public override void Enter()
    {
        playerBehaviour.GetAudioPort.OnSoundInfos(soundInfos.onEnter);
        playerBehaviour.anim.SetBool(Animator.StringToHash("Grounded"), false);

        SoundAlertInfo soundAlertInfo = new SoundAlertInfo
        {
            soundRange = playerBehaviour.PlayerData.GetAlertingRanges.jump,
            point = playerBehaviour.transform.position
        };

        SoundAlertCreation.CreateAlertPoint(soundAlertInfo,playerBehaviour.EnemyManagerPort);

    }
    public override void Exit()
    {
        playerBehaviour.anim.SetBool(Animator.StringToHash("Jump"), false);
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

    public override void OnDrawGizmos(PlayerBehaviour player)
    {
        if (gizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.transform.position, player.GetNormalPlayerData.GetAlertingRanges.jump);
            Gizmos.DrawWireSphere(player.transform.position, player.GetShrinkPlayerData.GetAlertingRanges.jump);
        }
    }

    [Serializable]
    private struct SoundInfos
    {   
        public SoundInfo[] onEnter;
    }
}
