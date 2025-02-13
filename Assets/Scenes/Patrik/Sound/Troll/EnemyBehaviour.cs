using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IComparable<EnemyBehaviour>
{
    
    [SerializeField] private RegistrationPort registrationPort = null;
    [SerializeField] protected Transform target = null;

    public Transform GetTarget => target;
    
    private Transform transform = null; 
    
    
    protected virtual void OnEnable()
    {
        registrationPort.OnRegister += RegisterTarget;
    }

    protected virtual void OnDisable()
    {
        registrationPort.OnRegister -= RegisterTarget;
    }

    private void RegisterTarget(RegistrationPort.TypeOfRegistration type ,GameObject newTarget)
    {
        if (type != RegistrationPort.TypeOfRegistration.Player) return;
        
        target = newTarget.transform;
    }
    
    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    protected virtual void Start()
    {
        registrationPort.OnRegister(RegistrationPort.TypeOfRegistration.Enemy, gameObject);
    }

    public float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, target.position);
    }
    
    public int CompareTo(EnemyBehaviour other)
    {
        return GetDistanceToPlayer().CompareTo(other.GetDistanceToPlayer());
    }
}
