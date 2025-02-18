using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "LevelHandling/Port/AudioPort")]
public class AudioPort : ScriptableObject
{
    public UnityAction<CharacterAudioData,MaterialComposition,Vector3> OnStep = delegate(CharacterAudioData arg0, MaterialComposition composition, Vector3 vector3) {  };
}
