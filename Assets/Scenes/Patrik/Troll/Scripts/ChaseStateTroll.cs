using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Vector3 direction = (TrollBehaviour.GetTarget.position - TrollBehaviour.gameObject.transform.position).normalized;
        Physics.Raycast(TrollBehaviour.gameObject.transform.position, direction,out RaycastHit hit);

        TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
        
        if (hit.collider != TrollBehaviour.GetTarget.GetComponent<Collider>())
        {
            TrollBehaviour.Transition(TrollBehaviour.SearchState);
        }
    }
}
