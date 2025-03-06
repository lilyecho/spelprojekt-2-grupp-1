using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[Serializable]
public class SearchStateTroll : TrollStates
{
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;
    
    public override void Enter()
    {
        TrollBehaviour.activeState = TrollBehaviour.States.Search;
        
        //Change pathfinding system so that trolls will get run over by more aggressive trolls - Chase or attack
        TrollBehaviour.GetNavMeshAgent.avoidancePriority = TrollBehaviour.GetTrollData.GetSearch.statePriority;
        TrollBehaviour.GetNavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        
        SetUpStateValuesInAgent(TrollBehaviour.GetTrollData.GetSearch);
    }

    public override void FixedUpdate()
    {
        if (TrollBehaviour.GetNavMeshAgent.remainingDistance <= 0.01f)
        {
            TrollBehaviour.Transition(TrollBehaviour.PatrolState);
        }
        
        Check4Player();
    }
    
    private void Check4Player()
    {
        if (TrollBehaviour.GetTarget == null) return;
        if (CheckIfTargetPositionIsWalkable()) return;
        if (!CheckTargetWithinAngleOfSight()) return;

        if (CheckIfRaycastHit())
        {
            TrollBehaviour.Transition(TrollBehaviour.ChaseState);
        }
    }
}
