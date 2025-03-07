using System;

[Serializable]
public struct SoundInfo
{
    public SoundAction action;
    
    
    
    
}

[Flags]
public enum SoundAction
{
    Create = 1,
    ChangeParameter = 2,
    Attach = 4,
    Play = 8,
    Remove = 16
}


