using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public enum Action2
{
    Play,
    Stop,
    SetParameter,
}

[Serializable]
public struct EventInfo2
{
    public EventReference eventReference;
    public Action action;
    public STOP_MODE stopMode;

    public bool attachedInstance;
    public bool attachedOneShot;
    public GameObject audioSource;

    [ParamRef] public string paramName;
    public float value;
}

