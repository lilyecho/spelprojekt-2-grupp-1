using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = FMOD.Debug;

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
