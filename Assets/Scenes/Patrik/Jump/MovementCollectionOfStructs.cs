using System;
using UnityEngine;

[Serializable]
public struct JumpParameters
{
    [SerializeField] private float jumpHeight;
    [SerializeField,Range(0,100)] private float keptMomentum;

    public float GetJumpHeight => jumpHeight;
}

[Serializable]
public struct MidAirForces
{
    //TODO custom
    [SerializeField] private float airCorrectionMagnitude;

    public float GetAppliedMagnitude => airCorrectionMagnitude;
}

[Serializable]
public struct SpeedRelated
{
    [SerializeField] public SpeedParameters sneak;
    [SerializeField] public SpeedParameters walk;
    [SerializeField] public SpeedParameters run;

    
}

[Serializable]
public struct SpeedParameters
{
    [SerializeField, Min(0)] public float speed;
    [SerializeField, Min(0.001f)] public float accTotalTime;
}
