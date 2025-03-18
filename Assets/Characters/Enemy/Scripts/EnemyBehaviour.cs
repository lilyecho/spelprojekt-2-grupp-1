using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    #region DragRefrences
    [Header("EnemyBehaviour")]
    [SerializeField] private EnemyManagerPort enemyManagerPort = null;
    [SerializeField] private RegistrationPort registrationPort = null;
    [SerializeField] private AudioPort audioPort = null;
    //[SerializeField] private SceneManagerPort sceneManagerPort = null;
    
    #endregion

    private PlayerBehaviour target = null;
    private Transform enemyTransform = null; 
    
    [CanBeNull] public PlayerBehaviour GetTarget => target;
    [CanBeNull] public Transform GetTargetTransform => target.transform;
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

    public virtual void Alerted(Vector3 alertPoint) {}

    private void RegisterTarget(RegistrationPort.TypeOfRegistration type ,GameObject newTarget)
    {
        if (type != RegistrationPort.TypeOfRegistration.Player) return;

        if (!newTarget.TryGetComponent(out PlayerBehaviour player))
        {
            throw new MissingComponentException("Player dont have PlayerBehaviour");
        }
        
        target = player;
    }
    
    protected virtual void Awake()
    {
        enemyTransform = GetComponent<Transform>();
    }

    protected virtual void Start()
    {
        enemyManagerPort.OnRegister(RegistrationPort.TypeOfRegistration.Enemy, gameObject);
    }

    
    /// <summary>
    /// Will return float.Max if not existing target
    /// </summary>
    /// <returns></returns>
    public bool GetDistanceToPlayer(out float? distance)
    {
        distance = null;
        if (target == null)
        {
            Debug.Log("Missing target");
            return false;
        }

        distance = Vector3.Distance(enemyTransform.position, target.transform.position);
        return true;
    }
}
