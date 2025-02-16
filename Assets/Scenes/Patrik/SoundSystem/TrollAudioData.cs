using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Troll/AudioData")]
public class TrollAudioData : ScriptableObject
{
    [SerializeField] private EventInfo ChaseMusicEvent;
    [SerializeField] private EventInfo ExitChaseMusicEvent;
    //[SerializeField] private EventInfo DistanceMusicEvent;
    [SerializeField] private EventInfo TrollFootsteps;
    [SerializeField] private EventInfo TrollEmote;
    public EventInfo GetChaseMusicEvent => ChaseMusicEvent;
    public EventInfo GetExitChaseMusicEvent => ExitChaseMusicEvent;
    public EventInfo GetTrollFootsteps => TrollFootsteps;
    public EventInfo GetTrollEmote => TrollEmote;
}
