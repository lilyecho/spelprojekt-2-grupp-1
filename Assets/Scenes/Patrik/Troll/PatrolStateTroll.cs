using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PatrolStateTroll : TrollStates
{
    [SerializeField] private TrollAlertPort trollAlertPort;
    [SerializeField] private GameObject pointHolder;
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private int patrolPointIndex;

    [SerializeField] private bool reCalibrate;
    
    public override void Awake()
    {
        GetAllPoints();
        RenamePoints();
    }

    public override void Enter()
    {
        //Events
        trollAlertPort.OnAlertedPosition += SearchAtAlertPoint;
        
        TrollBehaviour.activeState = TrollBehaviour.States.Patrol;
        SetTargetPoint();
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
        CheckSwapPatrolPoint();
    }

    private void SearchAtAlertPoint(Vector3 alertSourcePosition)
    {
        TrollBehaviour.GetNavMeshAgent.SetDestination(alertSourcePosition);
        TrollBehaviour.Transition(TrollBehaviour.SearchState);
    }
    
    private void CheckForPlayer()
    {
        float distance =
            Vector3.Distance(TrollBehaviour.GetTarget.position, TrollBehaviour.gameObject.transform.position);
        if ( distance <= TrollBehaviour.GetTrollData.GetSightRange)
        {
            Physics.Raycast(TrollBehaviour.gameObject.transform.position, TrollBehaviour.GetTarget.position,out RaycastHit hit);

            if (hit.collider == TrollBehaviour.GetTarget.GetComponent<Collider>())
            {
                TrollBehaviour.Transition(TrollBehaviour.ChaseState);
            }
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

    public override void OnValidate(TrollBehaviour trollBehaviour)
    {
        base.OnValidate(trollBehaviour);
        
        //patrolPointIndex %= patrolPoints.Count;
        
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
