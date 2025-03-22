using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "LevelHandling/Port/AudioPort")]
public class AudioPort : ScriptableObject
{
    //On steps, changes depending on object sending this data
    public UnityAction<SoundInfo,Transform> OnStep = delegate(SoundInfo soundInfo, Transform vector3) {  };
    
    //Music
    public UnityAction<bool> OnChased = delegate(bool arg0) {  };
    
    //Holy grail of this audioSystem
    public UnityAction<SoundInfo> OnSoundInfo = delegate(SoundInfo soundInfo) {  };
    public UnityAction<SoundInfo[]> OnSoundInfos = delegate(SoundInfo[] soundInfos) {  };
}
