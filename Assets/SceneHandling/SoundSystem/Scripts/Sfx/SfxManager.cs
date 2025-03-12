using System;
using System.Collections.Generic;
using FMODUnity;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    [SerializeField] private FmodParameterData fmodParameterData = null;
    [SerializeField] private AudioPort audioPort = null;
    
    [Header("Sfx-Related")]
    [SerializeField] private AudioHandler audioHandler = null;

    
    
    private void OnEnable()
    {
        audioPort.OnStep += CreateSound4Step;
        audioPort.OnJump += CreateSound4Jump;
    }

    private void OnDisable()
    {
        audioPort.OnStep -= CreateSound4Step;
        audioPort.OnJump -= CreateSound4Jump;
        
    }

    private void CreateSound4Step(SoundInfo soundInfo, Transform checkerTransform)
    {
        MaterialComposition material = SoundFromMovingOnMaterial.GetObjectMaterial(checkerTransform);

        soundInfo.action |= SoundInfo.SoundAction.ChangeParameter;
        //Todo flera parametrar 
        soundInfo.parameterName = fmodParameterData.GetMaterialParameter;
        soundInfo.parameterValue = (float)material;
        
        audioHandler.HandleSoundInfo(soundInfo);
    }

    private void CreateSound4Jump(EventReference eventReference, Vector3 jumpPos)
    {
        audioHandler.PlayOneShot(eventReference, jumpPos);
    }
    
}
