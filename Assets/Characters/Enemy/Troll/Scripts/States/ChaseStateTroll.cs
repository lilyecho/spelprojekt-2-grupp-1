using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Enemy.Troll.Scripts.States;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[Serializable]
public class ChaseStateTroll : TrollStates
{
    [SerializeField,Range(0.1f,10f)] private float totalChaseTime = 4f;
    
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;
    
    [Space, SerializeField] private SoundInfos soundInfos;

    
    private float currentTime = 0f;
    
    #region SoundInfos
    [Serializable]
    struct SoundInfos
    {
        public SoundInfo[] onScream;
    }
    #endregion
    
    public override void Enter()
    {
        //Inspector thing
        TrollBehaviour.activeState = TrollBehaviour.States.Chase;
        TrollBehaviour.Animator.SetBool(TrollBehaviour.chasingAP, true);
        TrollBehaviour.GetAudioPort.OnSoundInfos(soundInfos.onScream);
        TrollBehaviour.stateColor = Color.red;

        //Timer
        currentTime = totalChaseTime;
        
        //Change pathfinding system so that trolls will get run over by more aggressive trolls - Attack
        TrollBehaviour.GetNavMeshAgent.avoidancePriority = TrollBehaviour.GetTrollData.GetChase.statePriority;
        TrollBehaviour.GetNavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        
        TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTargetTransform.position);
        TrollBehaviour.GetEnemyManagerPort.OnChaseChange(ChangeValue.Increase);
        
        SetUpStateValuesInAgent(TrollBehaviour.GetTrollData.GetChase);
    }

    public override void Exit()
    {
        TrollBehaviour.GetEnemyManagerPort.OnChaseChange(ChangeValue.Decrease);
        TrollBehaviour.Animator.SetBool(TrollBehaviour.chasingAP, false);
    }

    public override void Update()
    {
        CheckTimerForChase();
    }

    public override void FixedUpdate()
    {
        //TrollEyes
        TrollStates newState = Check4Player(TrollBehaviour.GetEyes, TrollBehaviour.GetTrollData.GetAggressionRange);
        if (newState != this && newState != TrollBehaviour.attackState) newState = Check4Player(TrollBehaviour.GetLamp, TrollBehaviour.GetTrollData.GetAggressionRange);
        if (newState != this)
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTargetTransform.position);
            TrollBehaviour.Transition(newState);
            return;
        }
        
        TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTargetTransform.position);
    }

    private void CheckTimerForChase()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            TrollBehaviour.Transition(TrollBehaviour.patrolState);
            currentTime = totalChaseTime;
        }
    }
    
    private TrollStates Check4Player(Transform eyes, float range)
    {
        if (TrollBehaviour.GetTarget == null) return TrollBehaviour.patrolState;
        
        bool inRangeOfAggression = CheckTargetInRange(eyes,range);
        if (!inRangeOfAggression) return TrollBehaviour.patrolState;
        
        if (!CheckIfPositionIsWalkable(TrollBehaviour.GetTargetTransform.position, range))
        {
            return TrollBehaviour.searchState;
        }
        
        if (!CheckIfRaycastHit(eyes,range))
        {
            return TrollBehaviour.searchState;
        }
        
        if (CheckTargetInRange(TrollBehaviour.transform,TrollBehaviour.GetTrollData.GetAttackRange) && !TrollBehaviour.GetTarget.Attacked) // insight and close enough for attack
        {
            TrollBehaviour.GetTarget.Attacked = true;
            return TrollBehaviour.attackState;
        }
        
        return TrollBehaviour.chaseState;
    }
    
    public override void OnDrawGizmos(TrollBehaviour trollBehaviour)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(trollBehaviour.transform.position,trollBehaviour.GetTrollData.GetAttackRange);
    }
}
