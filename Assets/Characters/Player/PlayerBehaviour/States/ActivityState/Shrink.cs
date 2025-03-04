using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Shrink : State
{
    public Shrink(PlayerBehaviour playerBehaviour) : base(playerBehaviour) {}


    public override void Enter()
    {
        CreateEffect();
        
        playerBehaviour.ChangeState(playerBehaviour.idle);
    }

    private void CreateEffect()
    {
        
    }
}
