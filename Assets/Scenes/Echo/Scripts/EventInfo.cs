using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public enum Action
{
    Play,
    PlayOneShot,
    Stop,
    SetParameter,
}

[Serializable]
public struct EventInfo
{
    public EventReference eventReference;
    public Action action;
    public STOP_MODE stopMode;

    public bool attachedInstance;
    public bool attachedOneShot;
    public GameObject audioSource;

    [ParamRef] public string paramName;
    public float value;
    public bool ignoreSeekSpeed;
}

