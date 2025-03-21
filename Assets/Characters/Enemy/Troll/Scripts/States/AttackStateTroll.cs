using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Enemy.Troll.Scripts.States;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[Serializable]
public class AttackStateTroll : TrollStates
{
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;
    
    [Space, SerializeField] private SoundInfos soundInfos;
    
    #region SoundInfos

    [Serializable]
    struct SoundInfos
    {
        public SoundInfo[] onAttack;
    }

    #endregion
    
    public override void Enter()
    {
        //Inspector thing
        TrollBehaviour.activeState = TrollBehaviour.States.Attack;
        TrollBehaviour.stateColor = Color.green;
        TrollBehaviour.Animator.SetTrigger(TrollBehaviour.grabbingAP);
        
        //Update camera
        TrollBehaviour.CameraPort.OnTarget(TrollBehaviour.GetEyes);
        
        //Change pathfinding system so that other trolls will get run over by this troll and fight with others of the same for space
        TrollBehaviour.GetNavMeshAgent.avoidancePriority = TrollBehaviour.GetTrollData.GetAttack.statePriority;
        TrollBehaviour.GetNavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        TrollBehaviour.GetNavMeshAgent.isStopped = true;
        CatchPlayer();
    }

    public override void FixedUpdate()
    {
        if (TrollBehaviour.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Respawn();
        }
    }

    private void CatchPlayer()
    {
        StopPlayerMovement();
        TrollBehaviour.cameraTrollPort.AstridCaught(TrollBehaviour.transform, TrollBehaviour.cameraPosDuringAttack);
    }

    private void Respawn()
    {
        TrollBehaviour.CheckPointPort.Respawn();
        TrollBehaviour.cameraTrollPort.ResetCamera();
    }
    
    private void StopPlayerMovement()
    {
        TrollBehaviour.GetTarget.ChangeMovementActivation(false);
    }
}
