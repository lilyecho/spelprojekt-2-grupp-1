using System;
using System.Collections.Generic;
using FMODUnity;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    [Header("Ports"),SerializeField] private AudioPort audioPort = null;
    [Space,Header("Refs"),SerializeField] private EnemyManager enemyManager = null;
    [SerializeField] private AudioHandler audioHandler = null;

    [Header("Troll-Related")] [SerializeField]
    private float trollSoundCooldown;

    private float currentTime = 4;
    
    private void OnEnable()
    {
        audioPort.OnStep += CreateSound4Step;
    }

    private void OnDisable()
    {
        audioPort.OnStep -= CreateSound4Step;
    }

    private void FixedUpdate()
    {
        currentTime -= Time.fixedDeltaTime;
        if (currentTime <= 0)
        {
            RndTrollPatrollingSounds();
            currentTime = trollSoundCooldown;
        }
    }

    private void CreateSound4Step(SoundInfo soundInfo, Transform checkerTransform)
    {
        MaterialComposition material = SoundFromMovingOnMaterial.GetObjectMaterial(checkerTransform);

        soundInfo.action |= SoundInfo.SoundAction.ChangeParameter;
        //Todo flera parametrar 
        soundInfo.parameterName = "Material";
        soundInfo.parameterValue = (float)material;
        
        audioHandler.HandleSoundInfo(soundInfo);
    }

    private void RndTrollPatrollingSounds()
    {
        //enemyManager
    }
}
