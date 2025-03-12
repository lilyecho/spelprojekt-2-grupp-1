using System;
using System.Collections;
using System.Collections.Generic;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class SoundToAnimationStep : SoundToAnimation
{
    private bool activeSound = false;
    protected override void CheckPlaySound()
    {
        float currentValue = animator.GetFloat(hashedCurveName);
        
        if (currentValue == 0) return;
        
        if (!activeSound && currentValue >= activationValue)
        {
            activeSound = true;
            audioPort.OnStep(soundInfo, transform);
        }
        else if (currentValue < activationValue)
        {
            activeSound = false;
        }
    }
    
   
}
