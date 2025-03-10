using System;
using System.Collections;
using System.Collections.Generic;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class SoundToAnimationStep : SoundToAnimation
{
    private float testValue = 0.5f;
    private bool activeSound = false;
    protected override void CheckPlaySound()
    {
        float currentValue = animator.GetFloat(hashedCurveName);
        if (currentValue == 0) return;
        
        if (!activeSound && currentValue >= activationValue)
        {
            Debug.Log("Step");
            activeSound = true;
            audioPort.OnStep(soundInfo, transform);
        }
        else if (currentValue < activationValue)
        {
            Debug.Log("No sound anymore");
            activeSound = false;
        }
        
        

        
        
        
    }
    
   
}
