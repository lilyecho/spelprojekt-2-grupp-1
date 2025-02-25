using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Troll/TrollData")]
public class TrollData : ScriptableObject
{
    [FormerlySerializedAs("patrolState")]
    [Header("Movements")]
    [SerializeField] private StateMovementParameters patrolStateMovement;
    [SerializeField] private StateMovementParameters searchStateMovement;
    [SerializeField] private StateMovementParameters chaseStateMovement;
    
    [Space,Header("Senses")] 
    [SerializeField] private TrollSight trollSight;
    
    [Space,SerializeField, Min(0)] private float hearingRange;
    
    public StateMovementParameters GetPatrol => patrolStateMovement;
    public StateMovementParameters GetSearch => searchStateMovement;
    public StateMovementParameters GetChase => chaseStateMovement;
    public TrollSight GetSightData => trollSight;
    public float GetHearingRange => hearingRange;
    
}

[Serializable]
public struct StateMovementParameters
{
    [Min(0)]public float speed;
    [Min(0)]public float angularSpeed;
    [Min(0)]public float acceleration;
}

[Serializable]
public struct TrollSight
{
    [Min(0.01f)]public float range;
    [Min(0.01f)]public float angle;
}
