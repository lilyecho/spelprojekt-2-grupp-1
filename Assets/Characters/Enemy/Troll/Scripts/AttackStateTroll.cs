using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[Serializable]
public class AttackStateTroll : TrollStates
{
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;
    
    public override void Enter()
    {
        //Inspector thing
        TrollBehaviour.activeState = TrollBehaviour.States.Attack;
        TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTarget.position);
        
        StopPlayerMovement();
        TrollBehaviour.StartCoroutine(WaitAndDie(3));
    }

    private void StopPlayerMovement()
    {
        if (!TrollBehaviour.GetTarget.gameObject.TryGetComponent<Rigidbody>(out Rigidbody targetComp))
            throw new MissingComponentException("Target (aka player) doesn't have rigidbody!");

        targetComp.constraints = RigidbodyConstraints.FreezeAll;
    }

    private static IEnumerator WaitAndDie(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
