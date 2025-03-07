using System;

[Serializable]
public struct SoundInfo
{
    public SoundAction action;
    
    
    
    
}

[Flags]
public enum SoundAction
{
    Create,
    ChangeParameter,
    Attach,
    Play,
    Remove,
    Release,
        
}


