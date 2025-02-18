using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "LevelHandling/Port/AudioPort")]
public class AudioPort : ScriptableObject
{
    public UnityAction<CharacterAudioData,Transform> OnStep = delegate(CharacterAudioData arg0, Transform vector3) {  };
    
    public UnityAction<string, int> OnChangeGlobalParameter = delegate(string arg0, int i) {  };
}
