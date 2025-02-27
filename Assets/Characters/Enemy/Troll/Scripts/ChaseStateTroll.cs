using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[Serializable]
public class ChaseStateTroll : TrollStates
{
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;
    
    public override void Enter()
    {
        //Inspector thing
        TrollBehaviour.activeState = TrollBehaviour.States.Chase;
        
        TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
        TrollBehaviour.GetEnemyManagerPort.OnChaseChange(ChangeValue.Increase);
        
        SetUpStateValuesInAgent(TrollBehaviour.GetTrollData.GetChase);
    }

    public override void Exit()
    {
        TrollBehaviour.GetEnemyManagerPort.OnChaseChange(ChangeValue.Decrease);
    }

    public override void FixedUpdate()
    {
        Check4Player();
    }

    private void Check4Player()
    {
        bool inRangeOfAggression = CheckTargetInRange(TrollBehaviour.GetTrollData.GetSightData.range);
        if ( inRangeOfAggression && CheckTargetInRange(TrollBehaviour.GetTrollData.GetAttackRange)) // insight and close enough for attack
        {
            TrollBehaviour.Transition(TrollBehaviour.AttackState);
        }
        else if (inRangeOfAggression)
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
            return;
        }
        
        NavMeshPath path = new NavMeshPath();

        if (!CheckIfTargetPositionIsWalkable(out path))
        {
            TrollBehaviour.Transition(TrollBehaviour.SearchState);
            return;
        }

        if (!CheckIfRaycastHit())
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
            TrollBehaviour.Transition(TrollBehaviour.SearchState);
            return;
        }
            
        TrollBehaviour.GetNavMeshAgent.path = path;
    }
}
