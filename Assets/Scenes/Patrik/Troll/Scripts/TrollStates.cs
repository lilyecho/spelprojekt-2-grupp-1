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

    protected bool CheckTargetInRange()
    {
        float distance =
            Vector3.Distance(TrollBehaviour.GetTarget.position, TrollBehaviour.gameObject.transform.position);
        return distance <= TrollBehaviour.GetTrollData.GetSightRange;
    }

    protected bool CheckTargetWithinAngleOfSight()
    {
        Vector3 directionToPlayer = (TrollBehaviour.GetTarget.position - TrollBehaviour.gameObject.transform.position)
            .normalized;
        float angle = Vector3.Angle(TrollBehaviour.transform.forward, directionToPlayer);
        
        return angle <= TrollBehaviour.GetTrollData.GetSightAngle;
    }
}
