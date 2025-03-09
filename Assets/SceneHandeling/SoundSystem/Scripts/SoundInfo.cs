using System;
using FMODUnity;
using UnityEngine;

[Serializable]
public struct SoundInfo
{
    public SoundAction action;
    public EventReference eventReference;

    public SoundLocality locality;
    [ParamRef] public string parameterName;
    public float parameterValue;
    
    public PlayVariant playVariant;

    public LocationVariant locationVariant;
    public Transform locationTransform;

    public StopMode stopMode;
    
    public enum SoundLocality
    {
        Local,
        Global
    }
    
    public enum StopMode
    {
        AllowFadeout,
        Immediate
    }
    
    public enum PlayVariant
    {
        OneShot,
        Instance
    }
    
    public enum LocationVariant
    {
        Placement,
        Attached
    }
    
    [Flags]
    public enum SoundAction
    {
        Create = 1,
        ChangeParameter = 2,
        Location = 4,
        Play = 8,
        Remove = 16
    }
}




