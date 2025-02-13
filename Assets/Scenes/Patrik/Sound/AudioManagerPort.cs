using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "LevelHandling/Port/AudioManager")]
public class AudioManagerPort : ScriptableObject
{
    public UnityAction<bool> OnChased = delegate(bool arg0) {  };
}
