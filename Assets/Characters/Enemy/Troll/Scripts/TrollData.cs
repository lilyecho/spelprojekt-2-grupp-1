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
    
    [FormerlySerializedAs("trollSight")]
    [Space,Header("Senses")] 
    [SerializeField] private Sight trollTrollSight;
    [SerializeField] private Sight lampSight;
    
    [Space,SerializeField, Min(0)] private float aggressionRange;
    
    public StateParameters GetPatrol => patrolStateMovement;
    public StateParameters GetSearch => searchStateMovement;
    public StateParameters GetChase => chaseStateMovement;
    public StateParameters GetAttack => attackStateMovement;
    public float GetAttackRange => attackRange;
    public Sight GetTrollSight => trollTrollSight;
    public Sight GetLampSight => lampSight;
    public float GetAggressionRange => aggressionRange;

    private void OnValidate()
    {
        if (aggressionRange == 0)
        {
            Debug.Log("AggressionRange is zero will lose all aggression immediately");
        }
    }
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
public struct Sight
{
    [Min(0.01f)]public float range;
    [Min(0.01f)]public float angle;
}
