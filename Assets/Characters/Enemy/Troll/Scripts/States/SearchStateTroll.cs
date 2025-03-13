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
        TrollBehaviour.Animator.SetBool(TrollBehaviour.searchingAP, true);
        TrollBehaviour.stateColor = Color.yellow;
        
        //Change pathfinding system so that trolls will get run over by more aggressive trolls - Chase or attack
        TrollBehaviour.GetNavMeshAgent.avoidancePriority = TrollBehaviour.GetTrollData.GetSearch.statePriority;
        TrollBehaviour.GetNavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        
        SetUpStateValuesInAgent(TrollBehaviour.GetTrollData.GetSearch);
    }

    public override void Exit()
    {
        TrollBehaviour.Animator.SetBool(TrollBehaviour.searchingAP, false);
    }

    public override void FixedUpdate()
    {
        if (TrollBehaviour.GetNavMeshAgent.remainingDistance <= 0.01f)
        {
            TrollBehaviour.Transition(TrollBehaviour.PatrolState);
        }
        
        //TrollEyes
        if (Check4Player(TrollBehaviour.GetEyes, TrollBehaviour.GetTrollData.GetTrollSight.range,
                TrollBehaviour.GetTrollData.GetTrollSight.angle)) return;
        
        //Lampeyes
        if (Check4Player(TrollBehaviour.GetLamp, TrollBehaviour.GetTrollData.GetLampSight.range,
                TrollBehaviour.GetTrollData.GetLampSight.angle)) return;
    }
    
    private bool Check4Player(Transform eyes, float range, float angle)
    {
        if (TrollBehaviour.GetTarget == null){
            return false;
        }
        if (!CheckIfTargetPositionIsWalkable())
        {
            return false;
        } 
            
        if (!CheckTargetWithinAngleOfSight(eyes, angle)){
            return false;
        }

        if (CheckIfRaycastHit(eyes, range))
        {
            TrollBehaviour.Transition(TrollBehaviour.ChaseState);
            return true;
        }
        
        return false;
    }
}
