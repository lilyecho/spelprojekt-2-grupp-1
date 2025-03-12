using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;


[Serializable]
public class Falling : State
{
    float jumpBufferTimer;
    [SerializeField] private bool gizmos;


    #region AnimationParameters

    private int animationGrounded = Animator.StringToHash("Grounded");
    private int animationSuperJumpActivate = Animator.StringToHash("SuperJumpActivate");

    #endregion
    
    public override void Enter()
    {
        playerBehaviour.anim.SetBool(animationSuperJumpActivate, false);
        playerBehaviour.anim.SetBool(animationGrounded, false);
        playerBehaviour.ChangeJumpState(playerBehaviour.unableToJump);
        //jumpBufferTimer = playerBehaviour.PlayerData.GetJumpBufferDuration;
    }

    Quaternion targetRotation;

    public override void Update()
    {
        if (CheckForGround(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength))
        {
            CreateSoundAlert();
            playerBehaviour.ChangeState(playerBehaviour.idle);
            playerBehaviour.ChangeJumpState(playerBehaviour.normalJump);
        }
        
        if(playerBehaviour.intoJump)
        {
            jumpBufferTimer -= Time.deltaTime;
        }
        if(jumpBufferTimer <= 0 )
        {
            playerBehaviour.intoJump = false;
        }
    }

    protected override void CreateSoundAlert()
    {
        SoundAlertInfo soundAlertInfo = new SoundAlertInfo
        {
            soundRange = playerBehaviour.PlayerData.GetAlertingRanges.landing,
            point = playerBehaviour.transform.position
        };
            
        SoundAlertCreation.CreateAlertPoint(soundAlertInfo,playerBehaviour.EnemyManagerPort);
    }

    public override void FixedUpdate()
    {
        UpdateAirborneRotation2(playerBehaviour.rb, playerBehaviour.transform, ref playerBehaviour.currentVelocity, playerBehaviour.smoothTime);
        
        ApplyCorrectiveAirForces();
        
        //Gravity
        playerBehaviour.rb.AddForce(Vector3.down * playerBehaviour.PlayerData.GetGravityMagnitudeDown, ForceMode.Acceleration);
        
    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Glide())
            {
                playerBehaviour.ChangeState(playerBehaviour.gliding);
                playerBehaviour.intoJump = false;
            }
            else
            {
                jumpBufferTimer = playerBehaviour.PlayerData.GetJumpBufferDuration;
                playerBehaviour.intoJump = true;
            }
        }
    }
    
    public override void OnDrawGizmos(PlayerBehaviour player)
    {
        if (gizmos)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(player.transform.position, player.GetNormalPlayerData.GetAlertingRanges.landing);
            Gizmos.DrawWireSphere(player.transform.position, player.GetShrinkPlayerData.GetAlertingRanges.landing);
        }
    }
 
}
