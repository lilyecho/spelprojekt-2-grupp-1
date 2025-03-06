using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Troll/TrollData")]
public class TrollData : ScriptableObject
{
    [FormerlySerializedAs("patrolState")]
    [Header("Movements")]
    [SerializeField] private StateParameters patrolStateMovement;
    [SerializeField] private StateParameters searchStateMovement;
    [SerializeField] private StateParameters chaseStateMovement;
    [SerializeField] private StateParameters attackStateMovement;

    [SerializeField] private float attackRange = 1;
    
    [Space,Header("Senses")] 
    [SerializeField] private TrollSight trollSight;
    
    [Space,SerializeField, Min(0)] private float hearingRange;
    
    public StateParameters GetPatrol => patrolStateMovement;
    public StateParameters GetSearch => searchStateMovement;
    public StateParameters GetChase => chaseStateMovement;
    public StateParameters GetAttack => attackStateMovement;
    public float GetAttackRange => attackRange;
    public TrollSight GetSightData => trollSight;
    public float GetHearingRange => hearingRange;
    
}

[Serializable]
public struct StateParameters
{
    [Header("Movement")]
    [Min(0)]public float speed;
    [Min(0)]public float angularSpeed;
    [Min(0)]public float acceleration;
    
    [Space]
    [Range(0,99)] public int statePriority;
}

[Serializable]
public struct TrollSight
{
    [Min(0.01f)]public float range;
    [Min(0.01f)]public float angle;
}
