using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "LevelHandling/Port/AudioPort")]
public class AudioPort : ScriptableObject
{
    public UnityAction<CharacterAudioData,Transform> OnStep = delegate(CharacterAudioData arg0, Transform vector3) {  };

    public static UnityAction<string> OnTest = delegate(string a) { };

    public static void CallTest()
    {
        
    }
    
}
