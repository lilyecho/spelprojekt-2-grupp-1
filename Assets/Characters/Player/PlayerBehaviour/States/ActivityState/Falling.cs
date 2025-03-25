using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using SceneHandling.SoundSystem.Scripts;


[Serializable]
public class Falling : State
{
    float jumpBufferTimer;
    [SerializeField] private bool gizmos;
    private AirForceMode airForceMode;
    [SerializeField] private SoundInfos soundInfos;
    
    #region AnimationParameters

    private int animationGrounded = Animator.StringToHash("Grounded");
    private int animationSuperJumpActivate = Animator.StringToHash("SuperJumpActivate");
    private int animationSuperJumpRelease = Animator.StringToHash("SuperJumpRelease");

    #endregion
    
    [Serializable]
    private struct SoundInfos
    {   
        public SoundInfo[] onLanding;
    }
    
    public override void Enter()
    {
        GameManager.instance.HideBell();
        airForceMode = ConvertMovementModeToAirForceMode(playerBehaviour.movementMode);
        playerBehaviour.anim.SetBool(animationSuperJumpActivate, false);
        playerBehaviour.anim.SetBool(animationGrounded, false);
        playerBehaviour.ChangeJumpState(playerBehaviour.unableToJump);
        
    }

    Quaternion targetRotation;

    public override void Update()
    {
        if (CheckForGround(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength))
        {
            playerBehaviour.anim.SetBool(animationSuperJumpRelease, false);
            CreateSoundAlert(playerBehaviour.transform.position, playerBehaviour.PlayerData.GetAlertingRanges.landing);
            
            playerBehaviour.GetAudioPort.OnSoundInfos(soundInfos.onLanding);

            
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

    public override void FixedUpdate()
    {
        UpdateAirborneRotation2(playerBehaviour.rb, playerBehaviour.transform, ref playerBehaviour.currentVelocity, playerBehaviour.smoothTime);
        
        ApplyCorrectiveAirForces(airForceMode);
        
        //Gravity
        playerBehaviour.rb.AddForce(Vector3.down * playerBehaviour.PlayerData.GetGravityMagnitudeDown, ForceMode.Acceleration);
        
        ApplyHorizontalCounterForce();

        playerBehaviour.rb.velocity += PushDownSlopes(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength);
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

    /*
    public override void OnWASD(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerBehaviour.rb.drag = 2;
        }
        if (context.performed)
        {
            playerBehaviour.rb.drag = 0;
        }
    }

    public override void Exit()
    {
        playerBehaviour.rb.drag = 0;
    }
    */


    private Vector3  PushDownSlopes(Transform[] raycastPoints, float rayCastLength)
    {
        LayerMask layerToIgnore = (1 << 8) | (1 << 2) | (1 << 10);
        RaycastHit hit;
        Vector3 normal;
        Vector3 right;
        Vector3 slopeDirection;
        foreach (Transform t in raycastPoints)
        {
            if (Physics.Raycast(t.position, Vector3.down, out hit, rayCastLength, ~layerToIgnore))
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);

                if (angle > playerBehaviour.PlayerData.GetMaxRotationAngle)
                {
                    normal = hit.normal;
                    right = Vector3.Cross(Vector3.up, normal);
                    slopeDirection = Vector3.Cross(normal, right);
                    if (Vector3.Dot(slopeDirection, Vector3.down) < 0)
                    {
                        slopeDirection = -slopeDirection;
                    }

                    return slopeDirection.normalized;
                }
                
            }
            
        }
        return Vector3.zero;
    }

}
