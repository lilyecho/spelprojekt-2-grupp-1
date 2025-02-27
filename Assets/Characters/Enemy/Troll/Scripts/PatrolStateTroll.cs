using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PatrolStateTroll : TrollStates
{
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;
    
    [SerializeField] private TrollAlertPort trollAlertPort;
    [SerializeField,Tooltip("Parent for all patrolPoints")] private GameObject pointHolder;
    [SerializeField, ReadOnly] private List<Transform> patrolPoints;
    [SerializeField] private int patrolPointIndex;

    [SerializeField] private bool reCalibrate;

    public override void Awake(TrollBehaviour trollBehaviour)
    {
        base.Awake(trollBehaviour);
        
        GetAllPoints();
        RenamePoints();
    }

    public override void Enter()
    {
        Debug.LogError(TrollBehaviour);
        //Events
        trollAlertPort.OnAlertedPosition += SearchAtAlertPoint;
        
        TrollBehaviour.activeState = TrollBehaviour.States.Patrol;
        SetTargetPoint();
        SetUpStateValuesInAgent(TrollBehaviour.GetTrollData.GetPatrol);
    }

    public override void Exit()
    {
        //Events
        trollAlertPort.OnAlertedPosition -= SearchAtAlertPoint;
    }

    private void SetTargetPoint()
    {
        TrollBehaviour.GetNavMeshAgent.SetDestination(patrolPoints[patrolPointIndex%patrolPoints.Count].position);
    }
    
    public override void Update()
    {
        Check4Player();
        CheckSwapPatrolPoint();
    }

    private void SearchAtAlertPoint(Vector3 alertSourcePosition)
    {
        TrollBehaviour.GetNavMeshAgent.SetDestination(alertSourcePosition);
        TrollBehaviour.Transition(TrollBehaviour.SearchState);
    }
    
    private void Check4Player()
    {
        if (TrollBehaviour.GetTarget == null) return;
        if (!CheckTargetInRange(TrollBehaviour.GetTrollData.GetSightData.range)) return;
        if (!CheckTargetWithinAngleOfSight()) return;
        if (!CheckIfTargetPositionIsWalkable()) return;

        if (CheckIfRaycastHit())
        {
            TrollBehaviour.Transition(TrollBehaviour.ChaseState);
        }
    }
    
    private void CheckSwapPatrolPoint()
    {
        if (TrollBehaviour.GetNavMeshAgent.remainingDistance <= 0.01f)
        {
            patrolPointIndex = (patrolPointIndex+1)%patrolPoints.Count;
            SetTargetPoint();
        }
    }

    public override void OnValidate()
    {
        if (reCalibrate)
        {
            GetAllPoints();
            RenamePoints();
            reCalibrate = false;
        }
    }
    
    private void GetAllPoints()
    {
        List<Transform> temp = pointHolder.GetComponentsInChildren<Transform>().ToList();
        temp.RemoveAt(0);
        temp.TrimExcess();

        patrolPoints = new List<Transform>(temp);
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
            if (!patrolPoints[i] || !patrolPoints[(i+1)%patrolPoints.Count]) continue;
           
            Gizmos.DrawCube(patrolPoints[i].position, new Vector3(.5f,.5f,.5f));
            Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[(i+1)%patrolPoints.Count].position);
        }
    }
    
}
