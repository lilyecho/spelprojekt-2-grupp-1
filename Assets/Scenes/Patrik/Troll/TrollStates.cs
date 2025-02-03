using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrollStates
{
    protected TrollBehaviour TrollBehaviour = null;
    public virtual void Enter(){}
    public virtual void Exit(){}

    public virtual void OnValidate(TrollBehaviour trollBehaviour)
    {
        TrollBehaviour = trollBehaviour;
    }
}
