using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Enemy.Troll.Scripts.States;
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
        /*currentTimer -= Time.fixedDeltaTime;
        if (currentTimer <= 0)
        {
            TrollBehaviour.Transition(TrollBehaviour.patrolState);
            return;
        }*/
        
        //Troll
        TrollStates newState = Check4Player(TrollBehaviour.GetEyes, TrollBehaviour.GetTrollData.GetTrollSight.range);
        if (newState == this);
        else
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
            TrollBehaviour.Transition(newState);
            return;
        }
        
        newState = Check4Player(TrollBehaviour.GetLamp, TrollBehaviour.GetTrollData.GetLampSight.range);
        if (newState == this);
        else
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
            TrollBehaviour.Transition(newState);
            return;
        }
    }
    
    private TrollStates Check4Player(Transform eyes, float range)
    {
        bool inRangeOfAggression = CheckTargetInRange(eyes,TrollBehaviour.GetTrollData.GetAggressionRange);
        if (!inRangeOfAggression) return TrollBehaviour.patrolState;
        
        if (!CheckIfTargetPositionIsWalkable())
        {
            Debug.Log("not walkable - chase");
            return TrollBehaviour.searchState;
        }
        
        if (!CheckIfRaycastHit(eyes,range))
        {
            Debug.Log("not in raycast - chase");
            return TrollBehaviour.searchState;
        }
        
        if (CheckTargetInRange(TrollBehaviour.transform,TrollBehaviour.GetTrollData.GetAttackRange)) // insight and close enough for attack
        {
            Debug.Log("Close for attack - chase");
            return TrollBehaviour.attackState;
        }
        
        Debug.Log("default false - chase");
        return TrollBehaviour.chaseState;
    }
    
    public override void OnDrawGizmos(TrollBehaviour trollBehaviour)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(trollBehaviour.transform.position,trollBehaviour.GetTrollData.GetAttackRange);
    }
}
