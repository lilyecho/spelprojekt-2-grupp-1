using System;
using System.Collections;
using System.Collections.Generic;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class SoundToAnimation : MonoBehaviour
{
    [SerializeField] protected AudioPort audioPort;
    [SerializeField] protected SoundInfo soundInfo;
    
    [Space,Header("Curve-Related")]
    [SerializeField] protected string curveName;
    [SerializeField, Range(0,1)] protected float activationValue;
    
    protected Animator animator;
    protected int hashedCurveName;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        hashedCurveName = Animator.StringToHash(curveName);
    }

    protected virtual void Update()
    {
        CheckPlaySound();
    }

    protected virtual void CheckPlaySound()
    {
        if (animator.GetFloat(hashedCurveName) >= activationValue)
        {
            audioPort.OnSoundInfo(soundInfo);
        }
    }
}
