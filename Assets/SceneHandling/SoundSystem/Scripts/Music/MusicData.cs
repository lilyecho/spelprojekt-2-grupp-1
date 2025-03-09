using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Music/MusicData")]
public class MusicData : ScriptableObject
{
    [SerializeField] private EventReference mainMusic;

    public EventReference GetMainMusic => mainMusic;
}
