using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Characters/CharactersAudioData")]
public class CharacterAudioData : ScriptableObject
{
    [SerializeField] private EventReference audioMovement;
    [SerializeField] private EventReference audioJump;
    [SerializeField] private EventReference audioLand;
    [SerializeField] private EventReference audioEmote;
    [SerializeField] private EventReference audioPowers;

    public EventReference GetAudioMovement => audioMovement;
    public EventReference GetAudioJump => audioJump;
    public EventReference GetAudioLand => audioLand;
    public EventReference GetAudioEmote => audioEmote;
    public EventReference GetAudioPowers => audioPowers;
}
