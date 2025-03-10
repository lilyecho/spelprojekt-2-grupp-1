using System;
using System.Collections;
using System.Collections.Generic;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SoundToAnimation : MonoBehaviour
{
    [SerializeField] private AudioPort audioPort;
    [SerializeField] private SoundInfo soundInfo;
    [SerializeField] private string curveName;
    [SerializeField, Range(0,1)] private float activationValue;
    
    private Animator animator;
    private int hashedCurveName;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        hashedCurveName = Animator.StringToHash(curveName);
    }

    private void Update()
    {
        CheckPlaySound();
    }

    private void CheckPlaySound()
    {
        if (animator.GetFloat(hashedCurveName) >= activationValue)
        {
            audioPort.OnSoundInfo(soundInfo);
        }
    }
}
