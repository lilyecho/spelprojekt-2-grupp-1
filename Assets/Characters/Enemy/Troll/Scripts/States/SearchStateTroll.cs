using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Enemy.Troll.Scripts.States;
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
            TrollBehaviour.Transition(TrollBehaviour.lookAroundState);
        }
        
        //TrollEyes
        TrollStates newState = Check4Player(TrollBehaviour.GetEyes, TrollBehaviour.GetTrollData.GetTrollSight.range,
            TrollBehaviour.GetTrollData.GetTrollSight.angle);
        if (newState != this)
        {
            TrollBehaviour.Transition(newState);
            return;
        }
            
        //LampEyes
        newState = Check4Player(TrollBehaviour.GetLamp, TrollBehaviour.GetTrollData.GetLampSight.range,TrollBehaviour.GetTrollData.GetLampSight.angle);
        if (newState != this)
        {
            TrollBehaviour.Transition(newState);
            return;
        }
    }
    
    private TrollStates Check4Player(Transform eyes, float range, float angle)
    {
        if (TrollBehaviour.GetTarget == null) return TrollBehaviour.patrolState;
        if (CheckIfPlayerHidden()) return TrollBehaviour.searchState;
        if (!CheckIfPositionIsWalkable(TrollBehaviour.GetTargetTransform.position, range)) return TrollBehaviour.searchState;
        if (!CheckTargetWithinAngleOfSight(eyes, angle)) return TrollBehaviour.searchState;
        if (!CheckIfRaycastHit(eyes, range)) return TrollBehaviour.searchState;

        return TrollBehaviour.chaseState;
    }
}
