using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Characters/CharactersAudioData")]
public class CharacterAudioData : ScriptableObject
{
    [SerializeReference] private EventReference movementEvent;
}
