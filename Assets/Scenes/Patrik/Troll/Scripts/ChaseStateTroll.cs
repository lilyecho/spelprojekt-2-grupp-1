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

        bool test = NavMesh.CalculatePath(TrollBehaviour.GetNavMeshAgent.transform.position,
            TrollBehaviour.GetTarget.position, 1 << NavMesh.GetAreaFromName("Walkable"), path); //Has to do with binary 0,1,2,3 --> 1,2,4,8 1<< x moves the number 1 x ahead
        
        Debug.Log("On walkable: "+test);
        //Can't reach target
        if (!test) 
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
            TrollBehaviour.Transition(TrollBehaviour.SearchState);
            return;
        }
            
        Vector3 direction = (TrollBehaviour.GetTarget.position - TrollBehaviour.gameObject.transform.position).normalized;
        Physics.Raycast(TrollBehaviour.gameObject.transform.position, direction,out RaycastHit hit);
        
        //Can't see target
        if (hit.collider != TrollBehaviour.GetTarget.GetComponent<Collider>())
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
            TrollBehaviour.Transition(TrollBehaviour.SearchState);
            return;
        }
            
        TrollBehaviour.GetNavMeshAgent.path = path;
    }
}
