using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/SoundData")]
public class CharacterSounds : ScriptableObject
{
    [SerializeField] private CharacterActivity characterStates; 
    
    [Header("Walking")]
    [SerializeField] private AudioClip[] walkingDirt;
    [SerializeField] private AudioClip[] walkingGras;
    [SerializeField] private AudioClip[] walkingWood;
    [SerializeField] private AudioClip[] walkingStone;
    
    [Header("Running")]
    [SerializeField] private AudioClip[] runningDirt;
    [SerializeField] private AudioClip[] runningGras;
    [SerializeField] private AudioClip[] runningWood;
    [SerializeField] private AudioClip[] runningStone;
}
