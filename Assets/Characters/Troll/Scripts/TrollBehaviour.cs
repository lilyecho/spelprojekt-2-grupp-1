using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;


[RequireComponent(typeof(NavMeshAgent))]
public class TrollBehaviour : EnemyBehaviour
{
    public enum States
    {
        Null,
        Patrol,
        Chase,
        Search
    }

    #region DragReferences
    [SerializeField] private TrollData trollData;
    [SerializeField] private Transform eyes;
    

    [SerializeField] private TrollAudioData trollAudioData;
    #endregion
    
    private NavMeshAgent navMeshAgent;

    [ReadOnly] public States activeState = States.Null;

    #region States
    public PatrolStateTroll PatrolState = new PatrolStateTroll();
    public ChaseStateTroll ChaseState = new ChaseStateTroll();
    public SearchStateTroll SearchState = new SearchStateTroll();
    #endregion
    
    private TrollStates currentState = null;
    
    #region Getters & Setters
    public NavMeshAgent GetNavMeshAgent => navMeshAgent;
    
    public Transform GetEyes => eyes;
    public TrollData GetTrollData => trollData;
    public TrollAudioData GetAudioData => trollAudioData;
    
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();
        navMeshAgent = GetComponent<NavMeshAgent>();
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
        PatrolState.OnValidate(this);
        ChaseState.OnValidate(this);
        SearchState.OnValidate(this);
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
        VisualiseSight();
    }

    private void OnDrawGizmosSelected()
    {
        ChaseState.OnDrawGizmos();
        VisualiseAlert();
    }

    private void VisualiseAlert()
    {
        Gizmos.color = new Color(0f, 1f, 1f, .7f);
        Gizmos.DrawSphere(transform.position,trollData.GetHearingRange);
    }
    private void VisualiseSight() //Shame
    {
        //Only need x, z
        Vector3 forward = transform.forward;
        Gizmos.color = Color.red;

        //LeftSide
        Vector2 valuesForLeftSide = RotateVectorCounter(new Vector2(forward.x,forward.z), trollData.GetSightAngle);
        Vector3 leftSide = new Vector3(valuesForLeftSide.x, 0, valuesForLeftSide.y)*trollData.GetSightRange;

        Vector3 currentCubePos = transform.position + leftSide;
        Gizmos.DrawLine(transform.position, currentCubePos);
        Gizmos.DrawCube(currentCubePos, new Vector3(.1f,.1f,.1f));
        Vector3 pastCubePos = currentCubePos;
        
        //Points on frontline
        //LeftPoint
        Vector2 values4LeftPoint = RotateVectorCounter(new Vector2(forward.x,forward.z), trollData.GetSightAngle/2);
        Vector3 leftSidePoint = new Vector3(values4LeftPoint.x, 0, values4LeftPoint.y)*trollData.GetSightRange;
        currentCubePos = transform.position + leftSidePoint;
        
        Gizmos.DrawCube(currentCubePos, new Vector3(.1f,.1f,.1f));
        Gizmos.DrawLine(pastCubePos, currentCubePos);
        pastCubePos = currentCubePos;
        
        //CenterPoint
        currentCubePos = transform.position + transform.forward * trollData.GetSightRange;
        Gizmos.DrawCube(currentCubePos, new Vector3(.1f,.1f,.1f));
        Gizmos.DrawLine(pastCubePos, currentCubePos);
        pastCubePos = currentCubePos;
        
        //RightPoint
        Vector2 values4RightPoint = RotateVectorClock(new Vector2(forward.x,forward.z), trollData.GetSightAngle/2);
        Vector3 rightSidePoint = new Vector3(values4RightPoint.x, 0, values4RightPoint.y)*trollData.GetSightRange;
        
        currentCubePos = transform.position + rightSidePoint;
        Gizmos.DrawCube(currentCubePos, new Vector3(.1f,.1f,.1f));
        Gizmos.DrawLine(pastCubePos, currentCubePos);
        pastCubePos = currentCubePos;
        
        //RightSide
        Vector2 valuesForRightSide = RotateVectorClock(new Vector2(forward.x,forward.z), trollData.GetSightAngle);
        Vector3 rightSide = new Vector3(valuesForRightSide.x, 0, valuesForRightSide.y)*trollData.GetSightRange;

        currentCubePos = transform.position + rightSide;
        Gizmos.DrawLine(pastCubePos, currentCubePos);
        Gizmos.DrawCube(currentCubePos, new Vector3(.1f,.1f,.1f));
        pastCubePos = currentCubePos;
        
        //LastLineRight
        currentCubePos = transform.position;
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
}
