using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioHandler : MonoBehaviour
{
    private List<EventInstance> eventInstances = new List<EventInstance>();
    
    /*public void PlayOneShot(EventReference eventReference)
    {
        
    }
    public void PlayOneShot(EventReference eventReference, Vector3 placementPos)
    {
        
    }*/

    public void CreateInstance(EventReference eventReference, out EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(instance);
    }

    private void OnDestroy()
    {
        foreach (EventInstance instance in eventInstances)
        {
            instance.stop(STOP_MODE.IMMEDIATE);
            instance.release();
        }
    }
}
