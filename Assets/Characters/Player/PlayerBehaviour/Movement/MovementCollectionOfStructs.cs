using System;
using UnityEngine;

[Serializable]
public struct JumpParameters
{
    [SerializeField] private float jumpHeight;
    [SerializeField,Range(0,1)] private float keptMomentum;

    public float GetJumpHeight => jumpHeight;
    public float GetKeptMomentumPercentage => keptMomentum;
}

[Serializable]
public struct MidAirForces
{
    //TODO custom
    [SerializeField] private float airCorrectionMagnitude;
    [SerializeField] private float maximumSpeedInPlane;

    public float GetAppliedMagnitude => airCorrectionMagnitude;
    public float GetMaximumSpeed => maximumSpeedInPlane;
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
