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
        Vector3 direction = (TrollBehaviour.GetTarget.position - TrollBehaviour.gameObject.transform.position).normalized;
        Physics.Raycast(TrollBehaviour.gameObject.transform.position, direction,out RaycastHit hit);
        
        //Can't see target
        if (hit.collider != TrollBehaviour.GetTarget.GetComponent<Collider>())
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
            TrollBehaviour.Transition(TrollBehaviour.SearchState);
            return;
        }
        
        NavMeshPath path = new NavMeshPath();
        //Can't reach target
        if (!NavMesh.CalculatePath(TrollBehaviour.GetNavMeshAgent.transform.position, TrollBehaviour.GetTarget.position, 1 << NavMesh.GetAreaFromName("Walkable"), path))
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
            TrollBehaviour.Transition(TrollBehaviour.SearchState);
            return;
        }
        
        TrollBehaviour.GetNavMeshAgent.path = path;

    }
}
