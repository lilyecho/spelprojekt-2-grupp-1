using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Shrink : State
{
    public Shrink(PlayerBehaviour playerBehaviour) : base(playerBehaviour) {}

    private bool _active = false;
    

    public override void Enter()
    {
        CreateEffect();
        
        playerBehaviour.ChangeState(playerBehaviour.idle);
    }

    private void CreateEffect()
    {
        Particles();
        ReSize();
    }

    private void ReSize()
    {
        _active = !_active;
        if (_active)
        {
            playerBehaviour.transform.localScale = new Vector3(0.2f, .2f, .2f);
            playerBehaviour.PlayerData = playerBehaviour.GetShrinkPlayerData;
        }
        else
        {
            playerBehaviour.transform.localScale = new Vector3(1, 1, 1);
            playerBehaviour.PlayerData = playerBehaviour.GetNormalPlayerData;
        }
        
        
        
        
    }

    private void Particles()
    {
        playerBehaviour.GetOnShrinkParticleSystem.Play();
    }
    
}
