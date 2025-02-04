using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStateTroll : TrollStates
{
    public override void Enter()
    {
        TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
    }
    
    public override void FixedUpdate()
    {
        Check4Player();
    }

    private void Check4Player()
    {
        Physics.Raycast(TrollBehaviour.gameObject.transform.position, TrollBehaviour.GetTarget.position,out RaycastHit hit);

        TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
        
        if (hit.collider != TrollBehaviour.GetTarget.GetComponent<Collider>())
        {
            TrollBehaviour.Transition(TrollBehaviour.ChaseState);
        }
        
        
    }
}
