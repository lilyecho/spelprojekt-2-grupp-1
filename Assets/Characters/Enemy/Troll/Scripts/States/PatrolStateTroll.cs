using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SceneHandling.SoundSystem.Scripts;
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
        [SerializeField] private bool patrolSinglePoint;
        [SerializeField] private int patrolPointIndex;

        [SerializeField] private SoundInfos patrolSounds;
        
        [Space, SerializeField] private UnityEvent OnEnter;
        [SerializeField] private UnityEvent OnExit;

        private Vector3[] _internalPoints;
        private int _currentInternalPointIndex = 0;
        
        #region SoundInfos

        [Serializable]
        struct SoundInfos
        {
            public SoundInfo[] onRandomSounds;
        }

        #endregion

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

            //TrollBehaviour.StartCoroutine(Accelerate(TrollBehaviour.GetTrollData.GetPatrol.speed,2f));
            
            UpdateTargetPoint(patrolPointIndex);
            SetUpStateValuesInAgent(TrollBehaviour.GetTrollData.GetPatrol);
        }
    
        public override void Exit()
        {
            //Events
            trollAlertPort.OnAlertedPosition -= SearchAtAlertPoint;
            TrollBehaviour.Animator.SetBool(TrollBehaviour.patrollingAP, false);
        }

        private void SetTargetPoint(int currentPatrolIndex)
        {
            int nextPointIndex = currentPatrolIndex % TrollBehaviour.LocalPatrolPoints.Length;
            TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.LocalPatrolPoints[nextPointIndex]+TrollBehaviour.StartPos);
            
            _internalPoints =  TrollBehaviour.GetNavMeshAgent.path.corners;
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

        public override void FixedUpdate()
        {
            TrollBehaviour.Animator.SetFloat(TrollBehaviour.speedAP, TrollBehaviour.GetNavMeshAgent.velocity.magnitude);
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
            if (TrollBehaviour.LocalPatrolPoints.Length <= 0)
            {
                Debug.Log("Missing patrolpoints :"+TrollBehaviour.name);
                return;
            }
            
            if (TrollBehaviour.GetNavMeshAgent.remainingDistance <= 0.01f && patrolSinglePoint)
            {
                TrollBehaviour.transform.forward = TrollBehaviour.StartDir;
                //Rotate(TrollBehaviour.StartDir);
            }
            else if (TrollBehaviour.GetNavMeshAgent.remainingDistance <= 0.01f)
            {
                patrolPointIndex = (patrolPointIndex+1)%TrollBehaviour.LocalPatrolPoints.Length;
                UpdateTargetPoint(patrolPointIndex);
            }
        }

        private void Rotate(){
        }
        public override void OnDrawGizmos(TrollBehaviour trollBehaviour)
        {
            VisualizePoints();
            foreach (Vector3 pos in _internalPoints)
            {
                Debug.Log(pos);
                Gizmos.DrawCube(pos, new Vector3(0.1f,0.1f,0.1f));
            }
            Debug.Log("-----");
        }
    
        private void VisualizePoints()
        {
            if (TrollBehaviour.LocalPatrolPoints.Length < 1) return;
            if (TrollBehaviour.LocalPatrolPoints.Length == 1)
            {
                if (!Application.isPlaying)
                {
                    Gizmos.DrawCube( TrollBehaviour.transform.position+TrollBehaviour.LocalPatrolPoints[0], new Vector3(.5f,.5f,.5f));
                }
                else
                {
                    Gizmos.DrawCube( TrollBehaviour.StartPos+TrollBehaviour.LocalPatrolPoints[0], new Vector3(.5f,.5f,.5f));
                }
                return;
            }
        
            for (int i = 0; i < TrollBehaviour.LocalPatrolPoints.Length; i++)
            {
                if (!Application.isPlaying)
                {
                    Gizmos.DrawCube(TrollBehaviour.transform.position+TrollBehaviour.LocalPatrolPoints[i], new Vector3(.5f,.5f,.5f));
                    Gizmos.DrawLine(TrollBehaviour.transform.position+TrollBehaviour.LocalPatrolPoints[i], TrollBehaviour.transform.position+TrollBehaviour.LocalPatrolPoints[(i+1)%TrollBehaviour.LocalPatrolPoints.Length]);
                }
                else
                {
                    Gizmos.DrawCube(TrollBehaviour.StartPos+TrollBehaviour.LocalPatrolPoints[i], new Vector3(.5f,.5f,.5f));
                    Gizmos.DrawLine(TrollBehaviour.StartPos+TrollBehaviour.LocalPatrolPoints[i], TrollBehaviour.StartPos+TrollBehaviour.LocalPatrolPoints[(i+1)%TrollBehaviour.LocalPatrolPoints.Length]);
                }
            }
        }

        public void UpdateTargetPoint(int newPatrolIndex)
        {
            if (patrolSinglePoint)
            {
                TrollBehaviour.GetNavMeshAgent.SetDestination(TrollBehaviour.StartPos);
                return;
            }
            
            SetTargetPoint(newPatrolIndex);
            
            //Rotation
            Vector3 dir1 = TrollBehaviour.transform.forward;
            dir1.y = 0;
            Vector3 dir2 = (TrollBehaviour.LocalPatrolPoints[newPatrolIndex]+TrollBehaviour.StartPos - TrollBehaviour.transform.position).normalized;
            dir2.y = 0;
            
            float angle = Vector3.SignedAngle(dir1.normalized,dir2.normalized,Vector3.up);
            //Debug.Log(angle);
            
        }

        private void MakeRandomSound()
        {
            TrollBehaviour.GetAudioPort.OnSoundInfos(patrolSounds.onRandomSounds);
        }
        
        private void PatrolTurn()
        {
        
        }
    }
}
