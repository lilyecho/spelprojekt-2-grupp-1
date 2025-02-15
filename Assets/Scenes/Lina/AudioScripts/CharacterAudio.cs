using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Debug = UnityEngine.Debug;

[CreateAssetMenu(menuName = "Audio")]
public class CharacterAudio : ScriptableObject
{
    [SerializeField]private EventInfo[] audioMove, audioVoice;

 
}
