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

[Flags]
public enum MidAirForcesEnum
{
    Forward=1,
    Right=2,
    Backward=4,
    Left=8
}

[Serializable]
public struct MidAirForces
{
    [SerializeField] private MidAirForcesEnum forceToApply;
    
    //TODO custom
    [SerializeField] private Vector3 forward;
    [SerializeField] private Vector3 right;
    [SerializeField] private Vector3 backward;
    [SerializeField] private Vector3 left;

    public MidAirForcesEnum GetAppliedForces => forceToApply;
}
