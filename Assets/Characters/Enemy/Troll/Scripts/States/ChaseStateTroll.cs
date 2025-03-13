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

    private const float totalTime = 4;
    private float currentTimer;
    public override void Enter()
    {
        //Inspector thing
        TrollBehaviour.activeState = TrollBehaviour.States.Chase;
        TrollBehaviour.Animator.SetBool(TrollBehaviour.chasingAP, true);
        TrollBehaviour.stateColor = Color.red;
        currentTimer = totalTime;
        
        //Change pathfinding system so that trolls will get run over by more aggressive trolls - Attack
        TrollBehaviour.GetNavMeshAgent.avoidancePriority = TrollBehaviour.GetTrollData.GetChase.statePriority;
        TrollBehaviour.GetNavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        
        
        TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
        TrollBehaviour.GetEnemyManagerPort.OnChaseChange(ChangeValue.Increase);
        
        SetUpStateValuesInAgent(TrollBehaviour.GetTrollData.GetChase);
    }

    public override void Exit()
    {
        TrollBehaviour.GetEnemyManagerPort.OnChaseChange(ChangeValue.Decrease);
        TrollBehaviour.Animator.SetBool(TrollBehaviour.chasingAP, false);
    }

    public override void FixedUpdate()
    {
        currentTimer -= Time.fixedDeltaTime;
        if (currentTimer <= 0)
        {
            TrollBehaviour.Transition(TrollBehaviour.PatrolState);
            return;
        }
        
        //Troll
        if(Check4Player(TrollBehaviour.GetEyes, TrollBehaviour.GetTrollData.GetTrollSight.range)) return;
        //Lamp
        if(Check4Player(TrollBehaviour.GetLamp, TrollBehaviour.GetTrollData.GetLampSight.range)) return;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eyes"></param>
    /// <param name="range"></param>
    /// <returns>true if swap state</returns>
    private bool Check4Player(Transform eyes, float range)
    {
        bool inRangeOfAggression = CheckTargetInRange(eyes,TrollBehaviour.GetTrollData.GetAggressionRange);
        if ( inRangeOfAggression && CheckTargetInRange(TrollBehaviour.transform,TrollBehaviour.GetTrollData.GetAttackRange)) // insight and close enough for attack
        {
            TrollBehaviour.Transition(TrollBehaviour.AttackState);
            return true;
        }
        if (inRangeOfAggression)
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
            return false;
        }
        
        NavMeshPath path = new NavMeshPath();

        if (!CheckIfTargetPositionIsWalkable(out path))
        {
            TrollBehaviour.Transition(TrollBehaviour.SearchState);
            return true;
        }

        if (!CheckIfRaycastHit(eyes,range))
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
            TrollBehaviour.Transition(TrollBehaviour.SearchState);
            return true;
        }
            
        TrollBehaviour.GetNavMeshAgent.path = path;
        return false;
    }
    
    public override void OnDrawGizmos(TrollBehaviour trollBehaviour)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(trollBehaviour.transform.position,trollBehaviour.GetTrollData.GetAttackRange);
    }
}
