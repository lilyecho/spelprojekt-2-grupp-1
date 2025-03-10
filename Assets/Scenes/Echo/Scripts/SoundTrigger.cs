using System.Collections;
using System.Collections.Generic;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{

    [SerializeField] private AudioPort audioPort;
    [SerializeField] private SoundInfo soundInfo;
    
    public void OnInvoke()
    {

        audioPort.OnSoundInfo(soundInfo);

    }



}
