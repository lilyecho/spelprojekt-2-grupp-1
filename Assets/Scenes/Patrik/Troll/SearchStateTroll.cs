using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchStateTroll : TrollStates
{
    public override void Enter()
    {
        TrollBehaviour.activeState = TrollBehaviour.States.Search;
    }

    public override void FixedUpdate()
    {
        Check4Player();
        
        if (TrollBehaviour.GetNavMeshAgent.remainingDistance <= 0.01f)
        {
            TrollBehaviour.Transition(TrollBehaviour.PatrolState);
        }
    }
    
    private void Check4Player()
    {
        Physics.Raycast(TrollBehaviour.gameObject.transform.position, TrollBehaviour.GetTarget.position,out RaycastHit hit);
        
        if (hit.collider != TrollBehaviour.GetTarget.GetComponent<Collider>())
        {
            TrollBehaviour.Transition(TrollBehaviour.PatrolState);
        }
        else
        {
            TrollBehaviour.Transition(TrollBehaviour.ChaseState);
        }
    }
}
