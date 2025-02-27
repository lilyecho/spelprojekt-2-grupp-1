using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IComparable<EnemyBehaviour>
{
    #region DragRefrences
    [Header("EnemyBehaviour")]
    [SerializeField] private EnemyManagerPort enemyManagerPort = null;
    [SerializeField] private RegistrationPort registrationPort = null;
    [SerializeField] private AudioPort audioPort = null;
    //[SerializeField] private SceneManagerPort sceneManagerPort = null;
    
    #endregion

    private Transform target = null;
    private Transform enemyTransform = null; 
    
    public Transform GetTarget => target;
    public EnemyManagerPort GetEnemyManagerPort => enemyManagerPort;

    public AudioPort GetAudioPort => audioPort;
    
    
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
        enemyTransform = GetComponent<Transform>();
    }

    protected virtual void Start()
    {
        enemyManagerPort.OnRegister(RegistrationPort.TypeOfRegistration.Enemy, gameObject);
    }

    public float GetDistanceToPlayer()
    {
        return Vector3.Distance(enemyTransform.position, target.position);
    }
    
    public int CompareTo(EnemyBehaviour other)
    {
        return GetDistanceToPlayer().CompareTo(other.GetDistanceToPlayer());
    }
}
