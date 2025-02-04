using System;
using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(NavMeshAgent))]
public class TrollBehaviour : MonoBehaviour
{
    public enum States
    {
        Null,
        Patrol,
        Chase,
        Search
    }
    [SerializeField] private TrollData trollData;
    [SerializeField] private Transform eyes;

    [SerializeField] private TargetPort targetPort = null;
    [SerializeField] private Transform target = null;
    private NavMeshAgent navMeshAgent;

    public States activeState = States.Null; 
    
    public PatrolStateTroll PatrolState = new PatrolStateTroll();
    public ChaseStateTroll ChaseState = new ChaseStateTroll();
    public SearchStateTroll SearchState = new SearchStateTroll();

    private TrollStates currentState = null;

    public NavMeshAgent GetNavMeshAgent => navMeshAgent;
    public Transform GetTarget => target;
    public Transform GetEyes => eyes;
    public TrollData GetTrollData => trollData;
    
    private void OnEnable()
    {
        targetPort.OnTargetCreated += RegisterTarget;
        
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnDisable()
    {
        targetPort.OnTargetCreated -= RegisterTarget;
    }

    private void RegisterTarget(GameObject newTarget)
    {
        target = newTarget.transform;
    }
    
    private void Start()
    {
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

    private void OnDrawGizmosSelected()
    {
        PatrolState.OnDrawGizmos();
        ChaseState.OnDrawGizmos();
        VisualiseSight();
    }

    private void VisualiseSight()
    {
        //Only need x, z
        Vector3 forward = transform.forward;
        Gizmos.color = Color.red;

        SightLeftSide(forward);
        SightRightSide(forward);
    }

    private void SightLeftSide(Vector3 forward)
    {
        float leftSideX = forward.x * Mathf.Cos(Mathf.Deg2Rad * trollData.GetSightAngle) +
                          forward.z * -Mathf.Sin(Mathf.Deg2Rad * trollData.GetSightAngle);
        float leftSideZ = forward.x * Mathf.Sin(Mathf.Deg2Rad * trollData.GetSightAngle) +
                          forward.z * Mathf.Cos(Mathf.Deg2Rad * trollData.GetSightAngle);
        
        Vector3 leftSide = new Vector3(leftSideX, 0, leftSideZ) * trollData.GetSightRange;
        
        Gizmos.DrawLine(transform.position, transform.position+leftSide);
    }

    private void SightRightSide(Vector3 forward)
    {
        float rightSideX = forward.x * Mathf.Cos(Mathf.Deg2Rad * trollData.GetSightAngle) +
                           forward.z * Mathf.Sin(Mathf.Deg2Rad * trollData.GetSightAngle);
        float rightSideZ = forward.x * -Mathf.Sin(Mathf.Deg2Rad * trollData.GetSightAngle) +
                           forward.z * Mathf.Cos(Mathf.Deg2Rad * trollData.GetSightAngle);
        
        Vector3 rightSide = new Vector3(rightSideX, 0, rightSideZ) * trollData.GetSightRange;
        
        Gizmos.DrawLine(transform.position, transform.position+rightSide);
    }
}
