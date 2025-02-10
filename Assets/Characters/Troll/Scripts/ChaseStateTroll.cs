using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseStateTroll : TrollStates
{
    public override void Enter()
    {
        TrollBehaviour.activeState = TrollBehaviour.States.Chase;
        TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
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
