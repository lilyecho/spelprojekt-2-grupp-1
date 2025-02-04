using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    
    private bool CheckWithinDefinedAngleSight(Vector3 playerDirection)
    {
        float angle = Vector3.Angle(TrollBehaviour.transform.forward, playerDirection);
        return angle <= TrollBehaviour.GetTrollData.GetSightAngle;
    }

    protected bool CheckIfTargetIsSeen()
    {
        if (TrollBehaviour.GetTarget == null)return false;
        
        float distance =
            Vector3.Distance(TrollBehaviour.GetTarget.position, TrollBehaviour.gameObject.transform.position);
        if ( distance > TrollBehaviour.GetTrollData.GetSightRange) return false;

        Vector3 directionToPlayer = (TrollBehaviour.GetTarget.position - TrollBehaviour.gameObject.transform.position)
            .normalized;
        if (!CheckWithinDefinedAngleSight(directionToPlayer)) return false;

        return true;
    }
}
