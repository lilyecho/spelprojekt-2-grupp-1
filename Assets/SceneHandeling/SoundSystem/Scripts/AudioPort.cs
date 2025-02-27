using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "LevelHandling/Port/AudioPort")]
public class AudioPort : ScriptableObject
{
    //On steps, changes depending on object sending this data
    public UnityAction<CharacterAudioData,Transform> OnStep = delegate(CharacterAudioData arg0, Transform vector3) {  };
    
    public UnityAction<string, float> OnChangeGlobalParameter = delegate(string arg0, float i) {  };
    
    //For sounds that should loop
    public UnityAction<EventReference> OnCreate = delegate(EventReference reference) {  };
    
    
    public UnityAction<EventReference> OnStart = delegate(EventReference reference) {  };
    
    public UnityAction<EventReference, string, float> OnSetParameter = delegate(EventReference reference, string parameterName, float value) {  };
    
    public UnityAction<EventReference> OnRemove = delegate(EventReference reference) {  };
    
    //SFX focus
    public UnityAction<EventReference,Vector3> OnJump = delegate(EventReference reference, Vector3 arg1) {  };
        
}
