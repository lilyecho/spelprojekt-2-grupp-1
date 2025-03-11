using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[Serializable]
public class PatrolStateTroll : TrollStates
{
    [SerializeField] private TrollAlertPort trollAlertPort;
    [SerializeField,Tooltip("Parent for all patrolPoints")] private GameObject pointHolder;
    [SerializeField, ReadOnly] private List<Transform> patrolPoints;
    [SerializeField, ReadOnly] private int patrolPointIndex;

    [SerializeField] private bool reCalibrate;

    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;
    
    public override void Awake(TrollBehaviour trollBehaviour)
    {
        base.Awake(trollBehaviour);
        
        GetAllPoints();
        RenamePoints();
    }

    public override void Enter()
    {
        //Events
        trollAlertPort.OnAlertedPosition += SearchAtAlertPoint;
        
        TrollBehaviour.activeState = TrollBehaviour.States.Patrol;
        TrollBehaviour.stateColor = Color.blue;
        
        //Change pathfinding system so that trolls wont get stuck on the way to patrols
        TrollBehaviour.GetNavMeshAgent.avoidancePriority = TrollBehaviour.GetTrollData.GetPatrol.statePriority;
        TrollBehaviour.GetNavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        
        //Animation
        TrollBehaviour.Animator.SetBool("Moving", true);
        
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
        //TrollEyes
        if (Check4Player(TrollBehaviour.GetEyes, TrollBehaviour.GetTrollData.GetTrollSight.range,TrollBehaviour.GetTrollData.GetTrollSight.angle)) return;
        //LampEyes
        if(Check4Player(TrollBehaviour.GetLamp, TrollBehaviour.GetTrollData.GetLampSight.range,TrollBehaviour.GetTrollData.GetLampSight.angle)) return;
        CheckSwapPatrolPoint();
    }

    private void SearchAtAlertPoint(Vector3 alertSourcePosition)
    {
        TrollBehaviour.GetNavMeshAgent.SetDestination(alertSourcePosition);
        TrollBehaviour.Transition(TrollBehaviour.SearchState);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eyes"></param>
    /// <param name="range"></param>
    /// <param name="angle"></param>
    /// <returns>true if swap state</returns>
    private bool Check4Player(Transform eyes, float range, float angle)
    {
        if (TrollBehaviour.GetTarget == null) return false;
        if (!CheckTargetInRange(eyes,range)) return false;
        if (!CheckTargetWithinAngleOfSight(eyes,angle)) return false;
        if (!CheckIfTargetPositionIsWalkable()) return false;

        if (CheckIfRaycastHit(eyes,range))
        {
            TrollBehaviour.Transition(TrollBehaviour.ChaseState);
            return true;
        }

        return false;
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
