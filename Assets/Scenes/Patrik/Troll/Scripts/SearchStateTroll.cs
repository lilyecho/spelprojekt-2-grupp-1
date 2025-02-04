using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchStateTroll : TrollStates
{
    public override void Enter()
    {
        TrollBehaviour.activeState = TrollBehaviour.States.Search;
    }

    public override void FixedUpdate()
    {
        if (TrollBehaviour.GetNavMeshAgent.remainingDistance <= 0.01f)
        {
            TrollBehaviour.Transition(TrollBehaviour.PatrolState);
        }
        
        Check4Player();
        
    }
    
    private void Check4Player()
    {
        if (TrollBehaviour.GetTarget == null) return;
        
        NavMeshPath path = new NavMeshPath();
        //Can't reach target
        if (!NavMesh.CalculatePath(TrollBehaviour.GetNavMeshAgent.transform.position, TrollBehaviour.GetTarget.position, 1 << NavMesh.GetAreaFromName("Walkable"), path)) return;
        if (!CheckTargetWithinAngleOfSight()) return;
        
        Vector3 direction = (TrollBehaviour.GetTarget.position - TrollBehaviour.gameObject.transform.position).normalized;
        Physics.Raycast(TrollBehaviour.gameObject.transform.position, direction,out RaycastHit hit);
        
        if (hit.collider == TrollBehaviour.GetTarget.GetComponent<Collider>())
        {
            TrollBehaviour.Transition(TrollBehaviour.ChaseState);
        }
        
        
    }
}
