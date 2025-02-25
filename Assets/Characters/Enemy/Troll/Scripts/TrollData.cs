using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Troll/TrollData")]
public class TrollData : ScriptableObject
{
    [SerializeField] private TrollSpeeds trollSpeeds;
    
    [Space,Header("Senses")] 
    [SerializeField] private TrollSight trollSight;
    
    [Space,SerializeField, Min(0)] private float hearingRange;
    
    public TrollSpeeds GetSpeeds => trollSpeeds;
    public TrollSight GetSightData => trollSight;
    public float GetHearingRange => hearingRange;
    
}



[Serializable]
public struct TrollSpeeds
{
    [Min(0)]public float patrolSpeed;
    [Min(0)]public float searchSpeed;
    [Min(0)]public float chaseSpeed;
}

[Serializable]
public struct TrollSight
{
    [Min(0.01f)]public float range;
    [Min(0.01f)]public float angle;
}
