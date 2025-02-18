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
    
    public void PlayOneShot(EventReference eventReference)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventReference);
    }
    
    public void PlayOneShot(EventReference eventReference, Vector3 placementPos)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventReference);
        instance.set3DAttributes(placementPos.To3DAttributes());
        instance.start();
        instance.release();
    }

    /// <summary>
    /// Change parameter before the sound
    /// </summary>
    public void PlayOneShot(EventReference eventReference, Vector3 placementPos, string parameterName, int parameterValue)
    {
        //TODO performance-heavy
        EventInstance instance = RuntimeManager.CreateInstance(eventReference);

        instance.set3DAttributes(placementPos.To3DAttributes());
        
        instance.setParameterByName(parameterName,parameterValue);
        instance.start();
        instance.release();
    }
    
    /// <summary>
    /// Change parameters before the sound, but keep in mind the importance of index-relation between parameterNames and parameterValues 
    /// </summary>
    public void PlayOneShot(EventReference eventReference, Vector3 placementPos, Dictionary<string, int> parameterNamesAndValues)
    {
        if (parameterNamesAndValues.Count <= 0) throw new Exception("Elements in parameterNamesAndValues has to exist");
        
        //TODO performance-heavy
        EventInstance instance = RuntimeManager.CreateInstance(eventReference);
        
        instance.set3DAttributes(placementPos.To3DAttributes());
        foreach (var nameAndValue in parameterNamesAndValues)
        {
            instance.setParameterByName(nameAndValue.Key,nameAndValue.Value);
        }
        
        instance.start();
        instance.release();
    }

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
