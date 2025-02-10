using System;
using UnityEngine;

[Serializable]
public struct JumpParameters
{
    [SerializeField] private float jumpHeight;
    [SerializeField,Range(0,100)] private float momentumKept;

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
    [SerializeField, Min(0)] public float sneakSpeed;
    [SerializeField, Min(0)] public float walkSpeed;
    [SerializeField, Min(0)] public float runSpeed;
}
