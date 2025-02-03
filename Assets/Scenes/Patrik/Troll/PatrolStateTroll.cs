using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

[Serializable]
public class PatrolStateTroll : TrollStates
{
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private int patrolPointIndex;
    
    
    public override void Enter()
    {
        SetTargetPoint();
    }

    private void SetTargetPoint()
    {
        TrollBehaviour.GetNavMeshAgent.SetDestination(patrolPoints[patrolPointIndex%patrolPoints.Count].position);
    }
    
    public override void Update()
    {
        if (TrollBehaviour.GetNavMeshAgent.remainingDistance <= 0.01f)
        {
            patrolPointIndex = (patrolPointIndex+1)%patrolPoints.Count;
            SetTargetPoint();
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnValidate(TrollBehaviour trollBehaviour)
    {
        base.OnValidate(trollBehaviour);
        
        ValidatePatrolPoints();
    }
    
    private void ValidatePatrolPoints()
    {
        RemoveNullPoints();
        RenamePoints();
        
        //TODO do raycast for each point so that it is at the lowest point
    }

    private void RemoveNullPoints()
    {
        for (int i = patrolPoints.Count-1; i >= 0; i--)
        {
            if (patrolPoints[i] == null)
            {
                patrolPoints.RemoveAt(i);
            }
        }
        patrolPoints.TrimExcess();
    }
    
    private void RenamePoints()
    {
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            patrolPoints[i].name = $"PatrolPoint: {i}";
        }
    }
    
    public override void OnDrawGizmos()
    {
        VisualizePoints();
    }
    
    private void VisualizePoints()
    {
        if (patrolPoints.Count < 1) return;
        if (patrolPoints.Count == 1)
        {
            Gizmos.DrawCube(patrolPoints[0].position, new Vector3(.5f,.5f,.5f));
            return;
        }
        
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            Gizmos.DrawCube(patrolPoints[i].position, new Vector3(.5f,.5f,.5f));
            if (i == 0)
            {
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[^1].position);
            }
            else
            {
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i-1].position);
            }
        }
    }
    
}
