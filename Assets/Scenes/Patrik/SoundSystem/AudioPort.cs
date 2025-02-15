using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "LevelHandling/Port/AudioPort")]
public class AudioPort : ScriptableObject
{
    public UnityAction<EventReference,MaterialComposition,Vector3> OnStep = delegate(EventReference arg0, MaterialComposition composition, Vector3 vector3) {  };
}
