using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;


[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class TrollBehaviour : EnemyBehaviour
{
    #region DragReferences
    [Space,Header("TrollBehaviour")]
    [SerializeField] private TrollData trollData;
    [SerializeField] private Transform eyes;
    [SerializeField] private Transform lamp;
    
    [SerializeField] private CharacterAudio trollAudioData;

    [SerializeField] private CameraPort cameraPort;
    #endregion
    
    private NavMeshAgent navMeshAgent;

    [ReadOnly] public States activeState = States.Null;

    #region States
    [Space,Header("States")]
    public PatrolStateTroll PatrolState = new PatrolStateTroll();
    public ChaseStateTroll ChaseState = new ChaseStateTroll();
    public SearchStateTroll SearchState = new SearchStateTroll();
    public AttackStateTroll AttackState = new AttackStateTroll();
    #endregion
    
    private TrollStates currentState = null;

    private Animator animator = null;

    public Color stateColor = Color.black;
    
    #region Getters & Setters
    public NavMeshAgent GetNavMeshAgent => navMeshAgent;
    
    public Transform GetEyes => eyes;
    public Transform GetLamp => lamp;
    public TrollData GetTrollData => trollData;
    public CharacterAudio GetAudioData => trollAudioData;
    public CameraPort CameraPort => cameraPort;
    public Animator Animator => animator;
    
    #endregion

    public enum States
    {
        Null,
        Patrol,
        Chase,
        Search,
        Attack
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        
        base.Awake();
        PatrolState.Awake(this);
        ChaseState.Awake(this);
        SearchState.Awake(this);
        AttackState.Awake(this);
    }

    protected override void Start()
    {
        base.Start();
        InstantiateBeginState();
    }

    private void Update()
    {
        currentState.Update();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    private void OnValidate()
    {
        ValidateTrollBehaviour();
        
        PatrolState.OnValidate();
        ChaseState.OnValidate();
        SearchState.OnValidate();
        AttackState.OnValidate();
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
        if (trollAudioData == null)
        {
            Debug.LogWarning("Missing trollAudioData");
        }
    }

    private void InstantiateBeginState()
    {
        currentState = PatrolState;
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
        PatrolState.OnDrawGizmos();
        VisualiseSight(eyes.position, trollData.GetTrollSight);
        VisualiseSight(lamp.position, trollData.GetLampSight);
    }

    private void OnDrawGizmosSelected()
    {
        ChaseState.OnDrawGizmos();
        VisualiseAggressionRange();
    }

    private void VisualiseAggressionRange()
    {
        Gizmos.color = new Color(0f, 1f, 1f, .7f);
        Gizmos.DrawSphere(eyes.position,trollData.GetAggressionRange);
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

    public override void Alerted(Vector3 alertPoint)
    {
        if (activeState == States.Attack || activeState == States.Chase) return;
        
        navMeshAgent.SetDestination(alertPoint);
        Transition(ChaseState);
    }
}
