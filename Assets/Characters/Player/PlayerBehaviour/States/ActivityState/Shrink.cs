using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    public override void OnStateGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerBehaviour.transform.position + playerBehaviour.transform.up*0.3f, new Vector3(.2f,.5f,.7f));
    }
}
