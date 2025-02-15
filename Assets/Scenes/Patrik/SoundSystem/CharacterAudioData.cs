using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Characters/CharactersAudioData")]
public class CharacterAudioData : ScriptableObject
{
    [SerializeField] private EventReference audioMovement;

    public EventReference GetAudioMovement => audioMovement;
}
