using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;


namespace Characters.Enemy.Troll.Scripts.States
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class TrollBehaviour : EnemyBehaviour
    {
        #region DragReferences
        [Space,Header("TrollBehaviour")]
        [SerializeField] private TrollData trollData;
        [SerializeField] private Transform eyes;
        [SerializeField] private Transform lamp;

        [SerializeField] private CameraPort cameraPort = null;
        [SerializeField] private TimeManager timeManager = null;
        [SerializeField] private CheckPointPort checkPointPort = null;
        public CameraTrollPort cameraTrollPort;
        public Transform cameraPosDuringAttack;
        #endregion

        private Vector3 startPos;
    
        private bool _movementOn = true;
    
        private NavMeshAgent navMeshAgent;

        [ReadOnly] public States activeState = States.Null;

        [Space,SerializeField] private Vector3[] patrolPoints;
    
        #region States
        [Space,Header("States")] 
        public PatrolStateTroll patrolState = new PatrolStateTroll();
        public ChaseStateTroll chaseState = new ChaseStateTroll();
        public SearchStateTroll searchState = new SearchStateTroll();
        public LookAroundTrollState lookAroundState = new LookAroundTrollState();
        public AttackStateTroll attackState = new AttackStateTroll();
        #endregion
    
        private TrollStates currentState = null;

        private Animator animator = null;

        public Color stateColor = Color.black;

        #region AnimationParameters

        [HideInInspector] public int speedAP = Animator.StringToHash("Speed");
        [HideInInspector] public int chasingAP = Animator.StringToHash("Chasing");
        [HideInInspector] public int patrollingAP = Animator.StringToHash("Patrol");
        [HideInInspector] public int grabbingAP = Animator.StringToHash("Grabbing");
        [HideInInspector] public int lookAroundAP = Animator.StringToHash("Look Around");
        [HideInInspector] public int searchingAP = Animator.StringToHash("Searching");
        [HideInInspector] public int turnRightAP = Animator.StringToHash("TurnRight");
        [HideInInspector] public int turnLeftAP = Animator.StringToHash("TurnLeft");

        #endregion
    
        #region Getters & Setters

        public Vector3 StartPos => startPos;
        public NavMeshAgent GetNavMeshAgent => navMeshAgent;
        
        public ref Vector3[] LocalPatrolPoints => ref patrolPoints;
        public Transform GetEyes => eyes;
        public Transform GetLamp => lamp;
        public TrollData GetTrollData => trollData;
        public CameraPort CameraPort => cameraPort;
    
        public CheckPointPort CheckPointPort => checkPointPort;
        public Animator Animator => animator;
    
        #endregion
    
        public enum States
        {
            Null,
            Patrol,
            Chase,
            Search,
            LookAround,
            Attack
        }
    
        protected override void OnEnable()
        {
            timeManager.OnMovement += ChangeMovementActivation;
            checkPointPort.OnRespawn += Respawn;
        
            base.OnEnable();
            startPos = transform.position;
        }

        protected override void OnDisable()
        {
            timeManager.OnMovement -= ChangeMovementActivation;
            checkPointPort.OnRespawn -= Respawn;
        
            base.OnDisable();
        }

        protected override void Awake()
        {
            startPos = transform.position;
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        
            base.Awake();
            patrolState.Awake(this);
            chaseState.Awake(this);
            searchState.Awake(this);
            lookAroundState.Awake(this);
            attackState.Awake(this);
        }

        protected override void Start()
        {
            base.Start();
            InstantiateBeginState();
        }

        private void Update()
        {
            if (!_movementOn) return;
            currentState.Update();
        }

        private void FixedUpdate()
        {
            if (!_movementOn) return;
            currentState.FixedUpdate();
        }

        private void OnValidate()
        {
            ValidateTrollBehaviour();
        
            patrolState.OnValidate(this);
            chaseState.OnValidate(this);
            searchState.OnValidate(this);
            lookAroundState.OnValidate(this);
            attackState.OnValidate(this);
        }

        private void ValidateTrollBehaviour()
        {
            if (trollData == null)
            {
                Debug.LogWarning("Missing trollData");
            }
            if (eyes == null)
            {
                Debug.LogWarning("Missing eyes");
            }
            if (lamp == null)
            {
                Debug.LogWarning("Missing lamp");
            }
        }

        private void InstantiateBeginState()
        {
            currentState = patrolState;
            currentState.Enter();
        }
    
        public void Transition(TrollStates nextState)
        {
            currentState.Exit();
            currentState = nextState;
            currentState.Enter();
        }

        private void OnDrawGizmos()
        {
            patrolState.OnDrawGizmos(this);
            chaseState.OnDrawGizmos(this);
            attackState.OnDrawGizmos(this);
            VisualiseSight(eyes.position, trollData.GetTrollSight);
            VisualiseSight(lamp.position, trollData.GetLampSight);
        }

        private void OnDrawGizmosSelected()
        {
            chaseState.OnDrawGizmos(this);
            VisualiseAggressionRange();
        }

        #region Gizmos

        private void VisualiseAggressionRange()
        {
            Gizmos.color = new Color(0f, 1f, 1f, .7f);
            Gizmos.DrawWireSphere(eyes.position,trollData.GetAggressionRange);
        }
        private void VisualiseSight(Vector3 sightPoint, Sight sightData) //Shame
        {
            //Only need x, z
            Vector3 worldPos = sightPoint;
            Vector3 forward = transform.forward;
            Gizmos.color = stateColor;

            //LeftSide
            Vector2 valuesForLeftSide = RotateVectorCounter(new Vector2(forward.x,forward.z), sightData.angle);
            Vector3 leftSide = new Vector3(valuesForLeftSide.x, 0, valuesForLeftSide.y)*sightData.range;

            Vector3 currentCubePos = worldPos + leftSide;
            Gizmos.DrawLine(worldPos, currentCubePos);
            Gizmos.DrawCube(currentCubePos, new Vector3(.1f,.1f,.1f));
            Vector3 pastCubePos = currentCubePos;
        
            //Points on frontline
            //LeftPoint
            Vector2 values4LeftPoint = RotateVectorCounter(new Vector2(forward.x,forward.z), sightData.angle/2);
            Vector3 leftSidePoint = new Vector3(values4LeftPoint.x, 0, values4LeftPoint.y)*sightData.range;
            currentCubePos = worldPos + leftSidePoint;
        
            Gizmos.DrawCube(currentCubePos, new Vector3(.1f,.1f,.1f));
            Gizmos.DrawLine(pastCubePos, currentCubePos);
            pastCubePos = currentCubePos;
        
            //CenterPoint
            currentCubePos = worldPos + forward * sightData.range;
            Gizmos.DrawCube(currentCubePos, new Vector3(.1f,.1f,.1f));
            Gizmos.DrawLine(pastCubePos, currentCubePos);
            pastCubePos = currentCubePos;
        
            //RightPoint
            Vector2 values4RightPoint = RotateVectorClock(new Vector2(forward.x,forward.z), sightData.angle/2);
            Vector3 rightSidePoint = new Vector3(values4RightPoint.x, 0, values4RightPoint.y)*sightData.range;
        
            currentCubePos = worldPos + rightSidePoint;
            Gizmos.DrawCube(currentCubePos, new Vector3(.1f,.1f,.1f));
            Gizmos.DrawLine(pastCubePos, currentCubePos);
            pastCubePos = currentCubePos;
        
            //RightSide
            Vector2 valuesForRightSide = RotateVectorClock(new Vector2(forward.x,forward.z), sightData.angle);
            Vector3 rightSide = new Vector3(valuesForRightSide.x, 0, valuesForRightSide.y)*sightData.range;

            currentCubePos = worldPos + rightSide;
            Gizmos.DrawLine(pastCubePos, currentCubePos);
            Gizmos.DrawCube(currentCubePos, new Vector3(.1f,.1f,.1f));
            pastCubePos = currentCubePos;
        
            //LastLineRight
            currentCubePos = worldPos;
            Gizmos.DrawLine(pastCubePos, currentCubePos);
        }
    
        private Vector2 RotateVectorCounter(Vector2 inputVector, float angle)
        {
            if (angle <= 0) throw new ArgumentException("RotateVectorCounter can't and shouldn't handle angle less or equal to 0");
        
            float vectorX = inputVector.x * Mathf.Cos(Mathf.Deg2Rad * angle) +
                            inputVector.y * -Mathf.Sin(Mathf.Deg2Rad * angle);
            float vectorY = inputVector.x * Mathf.Sin(Mathf.Deg2Rad * angle) +
                            inputVector.y * Mathf.Cos(Mathf.Deg2Rad * angle);

            return new Vector2(vectorX, vectorY);
        }
        private Vector2 RotateVectorClock(Vector2 inputVector, float angle)
        {
            if (angle <= 0) throw new ArgumentException("RotateVectorCounter can't and shouldn't handle angle less or equal to 0");
        
            float vectorX = inputVector.x * Mathf.Cos(Mathf.Deg2Rad * angle) +
                            inputVector.y * Mathf.Sin(Mathf.Deg2Rad * angle);
            float vectorY = inputVector.x * -Mathf.Sin(Mathf.Deg2Rad * angle) +
                            inputVector.y * Mathf.Cos(Mathf.Deg2Rad * angle);

            return new Vector2(vectorX, vectorY);
        }

        #endregion
        
        public override void Alerted(Vector3 alertPoint)
        {
            if (activeState.HasFlag(States.Attack) || activeState.HasFlag(States.Chase)) return;
            if (!searchState.CheckIfPositionIsWalkable(alertPoint, trollData.GetAggressionRange)) return;
            
            navMeshAgent.SetDestination(alertPoint);
            Transition(searchState);
        }
    
        private void ChangeMovementActivation(bool nextValue)
        {
            _movementOn = nextValue;
            animator.speed = nextValue ? 1 : 0;

            if (nextValue)
            {
                switch (activeState)
                {
                    case States.Patrol:
                        navMeshAgent.speed = trollData.GetPatrol.speed;
                        break;
                    case States.Search:
                        navMeshAgent.speed = trollData.GetSearch.speed;
                        break;
                    case States.Chase:
                        navMeshAgent.speed = trollData.GetChase.speed;
                        break;
                    case States.Attack:
                        navMeshAgent.speed = trollData.GetAttack.speed;
                        break;
            
                    default:
                        Debug.Log("Missing state speed on movementUnpause");
                        break;
                }
            }
            else
            {
                navMeshAgent.speed = 0;
            }
        }
        
        public void Respawn()
        {
            transform.position = startPos;
            patrolState.UpdateTargetPoint(patrolState.PatrolPointIndex);
            navMeshAgent.isStopped = false;
            Transition(patrolState);
        }
    }
}
