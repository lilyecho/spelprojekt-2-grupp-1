using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Debug = UnityEngine.Debug;

[CreateAssetMenu(menuName = "Audio")]
public class CharacterAudio : ScriptableObject
{
    [SerializeField]private EventInfo audioSurface, audioVoice;
    
    public void playAudioSurface(MaterialComposition material)
    {
        audioSurface.value = material.GetHashCode();
        Console.WriteLine("Audio play: "+ audioSurface.value);
        AudioManager.Instance.InvokeEventInfo(audioSurface);
    }
    
    
}
