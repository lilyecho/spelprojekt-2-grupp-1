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
    
    private Transform target = null;
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
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        InstantiateBeginState();
    }

    private void Update()
    {
        currentState.Update();
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
    }
}
