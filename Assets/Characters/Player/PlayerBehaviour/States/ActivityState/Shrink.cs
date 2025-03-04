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
            playerBehaviour.PlayerData = playerBehaviour.GetShrinkPlayerData;
            playerBehaviour.transform.localScale = playerBehaviour.PlayerData.CharacterScale;
        }
        else
        {
            playerBehaviour.PlayerData = playerBehaviour.GetNormalPlayerData;
            playerBehaviour.transform.localScale = playerBehaviour.PlayerData.CharacterScale;
            
        }
    }

    private void Particles()
    {
        playerBehaviour.GetOnShrinkParticleSystem.Play();
    }
    
}
