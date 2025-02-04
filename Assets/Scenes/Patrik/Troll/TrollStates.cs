using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class TrollStates
{
    protected TrollBehaviour TrollBehaviour = null;
    
    public virtual void Awake(){}
    
    public virtual void Enter(){}
    public virtual void Exit(){}
    public virtual void Update(){}
    public virtual void FixedUpdate(){}

    public virtual void OnValidate(TrollBehaviour trollBehaviour)
    {
        TrollBehaviour = trollBehaviour;
    }

    public virtual void OnDrawGizmos()
    {
    }
    
}
