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
        base.Enter();
    }

    private void CreateEffect()
    {
        
    }
}
