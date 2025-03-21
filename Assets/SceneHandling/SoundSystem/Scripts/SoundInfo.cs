using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace SceneHandling.SoundSystem.Scripts
{
    /// <summary>
    /// Keep in mind currently no protection for when wrongly used.
    /// Example: Trying to get a valid locationTransform from when actions will not implement that transform-parameter
    /// </summary>
    [Serializable]
    public struct SoundInfo
    {
        public string soundImplementationName;
        public SoundAction action;
        public EventReference eventReference;

        public InstanceVariant instanceVariant;
        
        public SoundLocality locality;
        [ParamRef] public string parameterName;
        public float parameterValue;

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
            None,
            AllowFadeout,
            Immediate
        }
    
        public enum InstanceVariant
        {
            OneShot,
            SceneInstance,
            GameInstance
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
            Stop = 16
        }
    }
}




