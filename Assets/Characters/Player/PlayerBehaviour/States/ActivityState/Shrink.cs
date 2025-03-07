using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Shrink : State
{
    public Shrink(PlayerBehaviour playerBehaviour) : base(playerBehaviour) {}

    private bool _active = false;
    private Vector3 standardSize;

    public override void Enter()
    {
        ReSize();
        
        playerBehaviour.ChangeState(playerBehaviour.idle);
    }
    

    private void ReSize()
    {
        LayerMask layerMask = ~LayerMask.GetMask("Player", "InteractiveEnvironment");
        Debug.Log(Physics.OverlapBox(playerBehaviour.transform.position + playerBehaviour.transform.up*0.3f ,new Vector3(.2f,.5f,.7f)/2f,playerBehaviour.transform.rotation).Length);
        if (_active && Physics.OverlapBox(playerBehaviour.transform.position + playerBehaviour.transform.up*0.3f ,new Vector3(.2f,.5f,.7f)/2f,playerBehaviour.transform.rotation,layerMask).Length <= 0)
        {
            _active = false;
            Particles();
            playerBehaviour.PlayerData = playerBehaviour.GetNormalPlayerData;
            playerBehaviour.transform.localScale = playerBehaviour.PlayerData.CharacterScale;
            
        }
        else if( !_active)
        {
            _active = true;
            Particles();
            playerBehaviour.PlayerData = playerBehaviour.GetShrinkPlayerData;
            playerBehaviour.transform.localScale = playerBehaviour.PlayerData.CharacterScale;
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

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerBehaviour.transform.position + playerBehaviour.transform.up*0.3f, new Vector3(.2f,.5f,.7f));
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (_active)
        {
            Gizmos.DrawCube(playerBehaviour.transform.position + playerBehaviour.PlayerData.ShrinkPositionChange, new Vector3(0.1f,.1f,.1f));
        }
        else
        {
            Gizmos.DrawCube(playerBehaviour.transform.position + playerBehaviour.GetShrinkPlayerData.ShrinkPositionChange, new Vector3(0.1f,.1f,.1f));
        }
        
    }

    public override void OnValidate()
    {
        //Shrinking
        if (Vector3.Dot(playerBehaviour.PlayerData.ShrinkPositionChange, playerBehaviour.transform.up) <= 0)
        {
            Debug.LogWarning("Risk of clipping through floors and walls");
        }
        
        //Growing
        if (Vector3.Dot(playerBehaviour.GetShrinkPlayerData.ShrinkPositionChange, playerBehaviour.transform.up) <= 0)
        {
            Debug.LogWarning("Risk of clipping through floors and walls");
        }
    }
}
