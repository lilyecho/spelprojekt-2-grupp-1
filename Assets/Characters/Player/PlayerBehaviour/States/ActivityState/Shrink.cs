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

        _active = !_active;
        playerBehaviour.ChangeState(playerBehaviour.idle);
    }

    private void CreateEffect()
    {
        Particles();
        ReSize();
    }

    private void ReSize()
    {
        playerBehaviour.transform.localScale = _active ? new Vector3(1,1,1) : new Vector3(0.2f, .2f, .2f);
    }

    private void Particles()
    {
        playerBehaviour.GetOnShrinkParticleSystem.Play();
    }
    
}
