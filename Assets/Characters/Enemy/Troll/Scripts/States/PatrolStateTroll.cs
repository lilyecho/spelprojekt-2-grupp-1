using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Characters.Enemy.Troll.Scripts.States
{
    [Serializable]
    public class PatrolStateTroll : TrollStates
    {
        [SerializeField] private TrollAlertPort trollAlertPort;
        [SerializeField] private int patrolPointIndex;
    
        [SerializeField] private UnityEvent OnEnter;
        [SerializeField] private UnityEvent OnExit;
    
        public override void Enter()
        {
            //Events
            trollAlertPort.OnAlertedPosition += SearchAtAlertPoint;
        
            TrollBehaviour.activeState = TrollBehaviour.States.Patrol;
            TrollBehaviour.Animator.SetBool(TrollBehaviour.patrollingAP, true);
            TrollBehaviour.stateColor = Color.blue;
        
            //Change pathfinding system so that trolls wont get stuck on the way to patrols
            TrollBehaviour.GetNavMeshAgent.avoidancePriority = TrollBehaviour.GetTrollData.GetPatrol.statePriority;
            TrollBehaviour.GetNavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        
            SetTargetPoint();
            SetUpStateValuesInAgent(TrollBehaviour.GetTrollData.GetPatrol);
        }
    
        public override void Exit()
        {
            //Events
            trollAlertPort.OnAlertedPosition -= SearchAtAlertPoint;
            TrollBehaviour.Animator.SetBool(TrollBehaviour.patrollingAP, false);
        }

        private void SetTargetPoint()
        {
             
            int nextPointIndex = patrolPointIndex % TrollBehaviour.GetPatrolPoints.Length;
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.GetPatrolPoints[nextPointIndex]);
        }
    
        public override void Update()
        {
            //TrollEyes
            if (Check4Player(TrollBehaviour.GetEyes, TrollBehaviour.GetTrollData.GetTrollSight.range,TrollBehaviour.GetTrollData.GetTrollSight.angle)) return;
            //LampEyes
            if(Check4Player(TrollBehaviour.GetLamp, TrollBehaviour.GetTrollData.GetLampSight.range,TrollBehaviour.GetTrollData.GetLampSight.angle)) return;
            CheckSwapPatrolPoint();
        }

        private void SearchAtAlertPoint(Vector3 alertSourcePosition)
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(alertSourcePosition);
            TrollBehaviour.Transition(TrollBehaviour.SearchState);
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eyes"></param>
        /// <param name="range"></param>
        /// <param name="angle"></param>
        /// <returns>true if swap state</returns>
        private bool Check4Player(Transform eyes, float range, float angle)
        {
            if (TrollBehaviour.GetTarget == null) return false;
            if (!CheckTargetInRange(eyes,range)) return false;
            if (!CheckTargetWithinAngleOfSight(eyes,angle)) return false;
            if (!CheckIfTargetPositionIsWalkable()) return false;

            if (CheckIfRaycastHit(eyes,range))
            {
                TrollBehaviour.Transition(TrollBehaviour.ChaseState);
                return true;
            }

            return false;
        }
    
        private void CheckSwapPatrolPoint()
        {
            if (TrollBehaviour.GetPatrolPoints.Length <= 0)
            {
                Debug.Log("Missing patrolpoints :"+TrollBehaviour.name);
                return;
            }
            if (TrollBehaviour.GetNavMeshAgent.remainingDistance <= 0.01f)
            {
                patrolPointIndex = (patrolPointIndex+1)%TrollBehaviour.GetPatrolPoints.Length;
                SetTargetPoint();
            }
        }

        public override void OnDrawGizmos(TrollBehaviour trollBehaviour)
        {
            VisualizePoints();
        }
    
        private void VisualizePoints()
        {
            if (TrollBehaviour.GetPatrolPoints.Length < 1) return;
            if (TrollBehaviour.GetPatrolPoints.Length == 1)
            {
                Gizmos.DrawCube(TrollBehaviour.GetPatrolPoints[0], new Vector3(.5f,.5f,.5f));
                return;
            }
        
            for (int i = 0; i < TrollBehaviour.GetPatrolPoints.Length; i++)
            {
                Gizmos.DrawCube(TrollBehaviour.GetPatrolPoints[i], new Vector3(.5f,.5f,.5f));
                Gizmos.DrawLine(TrollBehaviour.GetPatrolPoints[i], TrollBehaviour.GetPatrolPoints[(i+1)%TrollBehaviour.GetPatrolPoints.Length]);
            }
        }
    }
}
