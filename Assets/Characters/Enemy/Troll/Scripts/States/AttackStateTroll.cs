using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Enemy.Troll.Scripts.States;
using UnityEngine;
using UnityEngine.AI;
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
        TrollBehaviour.stateColor = Color.green;
        
        //Update camera
        TrollBehaviour.CameraPort.OnTarget(TrollBehaviour.GetEyes);
        
        //Change pathfinding system so that other trolls will get run over by this troll and fight with others of the same for space
        TrollBehaviour.GetNavMeshAgent.avoidancePriority = TrollBehaviour.GetTrollData.GetAttack.statePriority;
        TrollBehaviour.GetNavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        
        TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetTargetTransform.position);
        //TrollBehaviour.GetNavMeshAgent.
        TrollBehaviour.cameraTrollPort.AstridCaught(TrollBehaviour.transform, TrollBehaviour.cameraPosDuringAttack);
        CatchPlayer();
        TrollBehaviour.Transition(TrollBehaviour.patrolState);
    }

    private void CatchPlayer()
    {
        //StopPlayerMovement();
        Respawn();
    }

    private void Respawn()
    {
        TrollBehaviour.CheckPointPort.Respawn();
        TrollBehaviour.cameraTrollPort.ResetCamera();
    }
    
    private void StopPlayerMovement()
    {
        if (!TrollBehaviour.GetTargetTransform.gameObject.TryGetComponent<Rigidbody>(out Rigidbody targetComp))
            throw new MissingComponentException("Target (aka player) doesn't have rigidbody!");

        targetComp.constraints = RigidbodyConstraints.FreezeAll;
    }
}
