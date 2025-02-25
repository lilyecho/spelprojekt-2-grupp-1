using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseStateTroll : TrollStates
{
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
        AudioManager.Instance.InvokeEventInfo(TrollBehaviour.GetAudioData.GetExitChaseMusicEvent);
    }

    public override void FixedUpdate()
    {
        Check4Player();
    }

    private void Check4Player()
    {
        NavMeshPath path = new NavMeshPath();

        if (CheckTargetInRange())
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
            return;
        }

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
