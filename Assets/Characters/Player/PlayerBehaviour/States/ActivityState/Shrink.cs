using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Shrink : State
{
    [SerializeField] private Vector3 shrinkPositionChange;
    [SerializeField] private Vector3 growPositionChange;
    [SerializeField] private float growColliderOffset;
    private bool _active = false;
    private Vector3 standardSize;

    
    public override void Enter()
    {
        ReSize();
        
        playerBehaviour.ChangeState(playerBehaviour.idle);
    }
    

    private void ReSize()
    {
        //Physics.OverlapBox(playerBehaviour.transform.position + playerBehaviour.transform.up*0.3f ,new Vector3(.2f,.5f,.7f)/2f,playerBehaviour.transform.rotation,layerMask).Length <= 0
        LayerMask layerMask = ~LayerMask.GetMask("Player", "InteractiveEnvironment");
        if (_active && Physics.OverlapBox(playerBehaviour.transform.position + playerBehaviour.transform.up*0.3f ,new Vector3(.2f,.5f,.7f)/2f,playerBehaviour.transform.rotation,layerMask).Length <= 0)
        {
            _active = false;
            Particles();
            playerBehaviour.PlayerData = playerBehaviour.GetNormalPlayerData;
            playerBehaviour.transform.localScale = playerBehaviour.PlayerData.CharacterScale;

            playerBehaviour.kameraPrototyp.cameraData = playerBehaviour.kameraPrototyp.normalCameraData;
            
        }
        else if( !_active)
        {
            _active = true;
            Particles();
            playerBehaviour.PlayerData = playerBehaviour.GetShrinkPlayerData;
            playerBehaviour.transform.localScale = playerBehaviour.PlayerData.CharacterScale;

            playerBehaviour.kameraPrototyp.cameraData = playerBehaviour.kameraPrototyp.shrinkedCameraData;
        }
    }

    private void Particles()
    {
        playerBehaviour.GetOnShrinkParticleSystem.Play();
    }

    private void ShrinkNewPosition(Vector3 positionChange)
    {
        playerBehaviour.transform.position += positionChange;
    }

    public override void OnDrawGizmos(PlayerBehaviour player)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(player.transform.position + growPositionChange.normalized * growColliderOffset, new Vector3(.2f,.5f,.7f));
    }

    public override void OnDrawGizmosSelected(PlayerBehaviour player)
    {
        if (_active) //GrowPos
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(player.transform.position + growPositionChange, new Vector3(0.1f,.1f,.1f));
            
        }
        else //ShrinkPos
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(player.transform.position + shrinkPositionChange, new Vector3(0.1f,.1f,.1f));
        }
    }
}
