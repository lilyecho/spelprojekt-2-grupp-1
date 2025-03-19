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


        #region Properties

        public int PatrolPointIndex => patrolPointIndex;

        #endregion
        
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
        
            SetTargetPoint(patrolPointIndex);
            SetUpStateValuesInAgent(TrollBehaviour.GetTrollData.GetPatrol);
        }
    
        public override void Exit()
        {
            //Events
            trollAlertPort.OnAlertedPosition -= SearchAtAlertPoint;
            TrollBehaviour.Animator.SetBool(TrollBehaviour.patrollingAP, false);
        }

        public void SetTargetPoint(int currentPatrolIndex)
        {
            int nextPointIndex = currentPatrolIndex % TrollBehaviour.WorldPatrolPoints.Length;
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.WorldPatrolPoints[nextPointIndex]);
        }
    
        public override void Update()
        {
            

            //TrollEyes
            TrollStates newState = Check4Player(TrollBehaviour.GetEyes, TrollBehaviour.GetTrollData.GetTrollSight.range,
                TrollBehaviour.GetTrollData.GetTrollSight.angle);
            if (newState != this)
            {
                TrollBehaviour.Transition(newState);
                return;
            }
            
            //LampEyes
            newState = Check4Player(TrollBehaviour.GetLamp, TrollBehaviour.GetTrollData.GetLampSight.range,TrollBehaviour.GetTrollData.GetLampSight.angle);
            if (newState != this)
            {
                TrollBehaviour.Transition(newState);
                return;
            }
            
            CheckSwapPatrolPoint();
        }
        
        
        private void SearchAtAlertPoint(Vector3 alertSourcePosition)
        {
            TrollBehaviour.GetNavMeshAgent.SetDestination(alertSourcePosition);
            TrollBehaviour.Transition(TrollBehaviour.searchState);
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eyes"></param>
        /// <param name="range"></param>
        /// <param name="angle"></param>
        /// <returns>true if swap state</returns>
        private TrollStates Check4Player(Transform eyes, float range, float angle)
        {
            if (TrollBehaviour.GetTarget == null) return TrollBehaviour.patrolState;
            if (CheckIfPlayerHidden()) return TrollBehaviour.patrolState;
            if (!CheckTargetInRange(eyes,range)) return TrollBehaviour.patrolState;
            if (!CheckTargetWithinAngleOfSight(eyes,angle)) return TrollBehaviour.patrolState;
            if (!CheckIfPositionIsWalkable(TrollBehaviour.GetTargetTransform.position, range)) return TrollBehaviour.patrolState;

            if (CheckIfRaycastHit(eyes,range))
            {
                return TrollBehaviour.chaseState;
            }

            return TrollBehaviour.patrolState;
        }
    
        private void CheckSwapPatrolPoint()
        {
            if (TrollBehaviour.WorldPatrolPoints.Length <= 0)
            {
                Debug.Log("Missing patrolpoints :"+TrollBehaviour.name);
                return;
            }
            if (TrollBehaviour.GetNavMeshAgent.remainingDistance <= 0.01f)
            {
                patrolPointIndex = (patrolPointIndex+1)%TrollBehaviour.WorldPatrolPoints.Length;
                SetTargetPoint(patrolPointIndex);
            }
        }

        public override void OnDrawGizmos(TrollBehaviour trollBehaviour)
        {
            VisualizePoints();
        }
    
        private void VisualizePoints()
        {
            if (TrollBehaviour.WorldPatrolPoints.Length < 1) return;
            if (TrollBehaviour.WorldPatrolPoints.Length == 1)
            {
                Gizmos.DrawCube(TrollBehaviour.WorldPatrolPoints[0], new Vector3(.5f,.5f,.5f));
                return;
            }
        
            for (int i = 0; i < TrollBehaviour.WorldPatrolPoints.Length; i++)
            {
                Gizmos.DrawCube(TrollBehaviour.WorldPatrolPoints[i], new Vector3(.5f,.5f,.5f));
                Gizmos.DrawLine(TrollBehaviour.WorldPatrolPoints[i], TrollBehaviour.WorldPatrolPoints[(i+1)%TrollBehaviour.WorldPatrolPoints.Length]);
            }
        }
    }
}
