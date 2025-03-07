using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SFXParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private EventReference audio;

    public void OnParticleTrigger()
    {
        RuntimeManager.PlayOneShot(audio);
        Debug.Log("Play: " + audio.ToString());
    }
    
}
