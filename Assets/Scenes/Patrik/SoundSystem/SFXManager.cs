using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class SFXManager : TempAudioManager
{
    [SerializeField] private AudioPort audioPort = null;
    [SerializeField] private FmodParameterData parameters = null;

    private void OnEnable()
    {
        audioPort.OnStep += CreateSound4Step;
    }

    private void OnDisable()
    {
        audioPort.OnStep -= CreateSound4Step;
    }

    private void CreateSound4Step(EventReference audioEvent, MaterialComposition material, Vector3 position)
    {
        //TODO performance-heavy
        EventInstance instance = RuntimeManager.CreateInstance(audioEvent);

        instance.set3DAttributes(position.To3DAttributes());
        //instance.setParameterByName(parameters.GetMaterialParameter,(int)material);
        instance.start();
        instance.release();
    }
}
