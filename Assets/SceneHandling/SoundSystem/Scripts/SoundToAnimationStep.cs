using System;
using System.Collections;
using System.Collections.Generic;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class SoundToAnimationStep : SoundToAnimation
{
    protected override void CheckPlaySound()
    {
        if (Math.Abs(animator.GetFloat(hashedCurveName) - activationValue) < 0.1)
        {
            Debug.Log("Stepsound");
            audioPort.OnStep(soundInfo, transform);
        }
    }
}
