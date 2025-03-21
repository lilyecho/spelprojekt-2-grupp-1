using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using SceneHandling.SoundSystem.Scripts;

[Serializable]
public class Idle : State
{
    float time = 0f;
    Vector3 normal;
    [SerializeField] private SoundInfos soundInfos;
    
    [Serializable]
    private struct SoundInfos
    {   
        public SoundInfo[] onEnter;
        public SoundInfo[] onExit;
    }    
    
    public override void Enter()
    {
        playerBehaviour.anim.SetBool(Animator.StringToHash("Grounded"), true);
        
        if(playerBehaviour.moveInput == Vector2.zero)
        {
            playerBehaviour.rb.velocity = Vector3.zero;
            playerBehaviour.GetAudioPort.OnSoundInfos(soundInfos.onEnter);
        }
        
        if (playerBehaviour.moveInput != Vector2.zero)
        {
            playerBehaviour.GetAudioPort.OnSoundInfos(soundInfos.onExit);
            switch (playerBehaviour.movementMode)
            {
                case PlayerBehaviour.MovementMode.SNEAK:
                    playerBehaviour.ChangeState(playerBehaviour.sneaking);
                    break;
                case PlayerBehaviour.MovementMode.RUN:
                    playerBehaviour.ChangeState(playerBehaviour.running);
                    break;
                case PlayerBehaviour.MovementMode.WALK:
                    playerBehaviour.ChangeState(playerBehaviour.walking);
                    break;
            }
        }
    }

    public override void Update()
    {
        //playerBehaviour.RotateCharacter(playerBehaviour.moveDir);
        normal = GetSurfaceNormal(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength);
        //playerBehaviour.transform.rotation = AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, time, Vector3.up);
        //playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, normal,
        //                                                        playerBehaviour.PlayerData.GetSlopeCheckerLength, playerBehaviour.PlayerData.GetMaxRotationAngle), time);
        time = time + Time.deltaTime;
        if (!CheckForGround(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength * 1.5f))
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);
        }
    }

    public override void FixedUpdate()
    {
        if (playerBehaviour.movementMode == PlayerBehaviour.MovementMode.SNEAK)
        {
            playerBehaviour.anim.SetBool(Animator.StringToHash("Sneaking"), true);
        }
        else
        {
            playerBehaviour.anim.SetBool(Animator.StringToHash("Sneaking"), false);
        }
        
        /*
        playerBehaviour.transform.rotation = playerBehaviour.RotateCharacter(playerBehaviour.moveDir) * AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, normal,
                                             playerBehaviour.PlayerData.GetSlopeCheckerLength, playerBehaviour.PlayerData.GetMaxRotationAngle);
        */
        playerBehaviour.moveDir = Vector3.ProjectOnPlane(playerBehaviour.moveDir, normal).normalized;


        playerBehaviour.rb.AddForce(-normal * 9.81f, ForceMode.Acceleration);
        
    }

    
    public override void OnWASD(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //playerBehaviour.ChangeState(playerBehaviour.walkState);


            switch (playerBehaviour.movementMode)
            {
                case PlayerBehaviour.MovementMode.SNEAK:
                    playerBehaviour.ChangeState(playerBehaviour.sneaking);
                    break;
                case PlayerBehaviour.MovementMode.RUN:
                    playerBehaviour.ChangeState(playerBehaviour.running);
                    break;
                case PlayerBehaviour.MovementMode.WALK:
                    playerBehaviour.ChangeState(playerBehaviour.walking);
                    break;
            }
        }
    }

    public override void OnShrink(InputAction.CallbackContext context)
    {
        if (context.performed && playerBehaviour.GetAbilities.HasFlag(AbilityData.Abilities.Shrink))
        {
            Debug.Log("ChangeToShrink");
            playerBehaviour.ChangeState(playerBehaviour.shrink);
        }
    }
}
