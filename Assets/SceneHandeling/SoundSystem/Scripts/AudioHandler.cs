using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] private AudioPort audioPort = null;
    [SerializeField] private FmodParameterData parameterData = null;
    private Dictionary<GUID,EventInstance> dictionaryGuidInstances = new Dictionary<GUID, EventInstance>();

    private void OnEnable()
    {
        audioPort.OnChangeGlobalParameter += ChangeGlobalParameter;
        audioPort.OnCreate += CreateInstance;
        audioPort.OnStart += PlayInstance;
        audioPort.OnSetParameter += ChangeLocalParameter;
        audioPort.OnRemove += RemoveInstance;
    }

    private void OnDisable()
    {
        audioPort.OnChangeGlobalParameter -= ChangeGlobalParameter;
        audioPort.OnCreate -= CreateInstance;
        audioPort.OnStart -= PlayInstance;
        audioPort.OnSetParameter -= ChangeLocalParameter;
        audioPort.OnRemove -= RemoveInstance;
    }

    private void ChangeGlobalParameter(string parameterName, int value)
    {
        RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
    }
    
    private void CreateInstance(EventReference eventReference)
    {
        GUID eventGUID = eventReference.Guid;
        if (dictionaryGuidInstances.ContainsKey(eventGUID))
        {
            return;
        }
        dictionaryGuidInstances[eventReference.Guid] = RuntimeManager.CreateInstance(eventReference);
    }

    private void PlayInstance(EventReference eventReference)
    {
        if (!TryGetInstance(eventReference, out EventInstance instance)) return;

        instance.start();
    }
    
    private void ChangeLocalParameter(EventReference reference, string parameterName, int value)
    {
        if (!dictionaryGuidInstances.ContainsKey(reference.Guid)) return;
        
        dictionaryGuidInstances[reference.Guid].setParameterByName(parameterName, value);
    }

    private void RemoveInstance(EventReference eventReference)
    {
        GUID eventGUID = eventReference.Guid;
        if (!TryGetInstance(eventGUID, out EventInstance instance)) return;
        
        dictionaryGuidInstances.Remove(eventGUID);
        //TODO hårdkodat
        instance.stop(STOP_MODE.ALLOWFADEOUT);
        instance.release();
        
    }
    
    private bool TryGetInstance(GUID eventGUID, out EventInstance instance)
    {
        instance = new EventInstance();
        if (!dictionaryGuidInstances.ContainsKey(eventGUID)) return false;

        instance = dictionaryGuidInstances[eventGUID];
        return true;
    }
    private bool TryGetInstance(EventReference eventReference, out EventInstance instance)
    {
        GUID eventGUID = eventReference.Guid;
        instance = new EventInstance();
        if (!dictionaryGuidInstances.ContainsKey(eventGUID)) return false;

        instance = dictionaryGuidInstances[eventGUID];
        return true;
    }
    
    public void PlayOneShot(EventReference eventReference)
    {
        RuntimeManager.PlayOneShot(eventReference);
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
    
    public bool TryCreateInstance(EventReference eventReference)
    {
        GUID eventGUID = eventReference.Guid;
        if (dictionaryGuidInstances.ContainsKey(eventGUID))
        {
            return false;
        }
        dictionaryGuidInstances[eventReference.Guid] = RuntimeManager.CreateInstance(eventReference);
        return true;
    }
    
    public bool TryCreateInstance(EventReference eventReference, out EventInstance instance)
    {
        GUID eventGUID = eventReference.Guid;
        if (dictionaryGuidInstances.ContainsKey(eventGUID))
        {
            instance = new EventInstance();
            return false;
        }
        
        instance = RuntimeManager.CreateInstance(eventReference);
        dictionaryGuidInstances[eventReference.Guid] = instance;
        return true;
    }

    public bool TryChangeLocalParameter(EventReference reference, string parameterName, int value)
    {
        if (dictionaryGuidInstances.ContainsKey(reference.Guid))
        {
            dictionaryGuidInstances[reference.Guid].setParameterByName(parameterName, value);
            return true;
        }
        return false;
    }
    
    public void TryChangeGlobalParameter(string parameterName, int value)
    {
        try
        {
            RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
        }
        catch (Exception e)
        {
            Debug.LogError("Missing global parameter");
        }
    }

    private bool TryStopSound(EventReference eventReference)
    {
        GUID eventGUID = eventReference.Guid;
        if (dictionaryGuidInstances.ContainsKey(eventGUID))
        {
            dictionaryGuidInstances.Remove(eventGUID);
            return true;
        }

        return false;
    }
    
    public bool TryStartSound(EventReference eventReference)
    {
        GUID eventGUID = eventReference.Guid;
        if (dictionaryGuidInstances.ContainsKey(eventGUID))
        {
            dictionaryGuidInstances[eventGUID].start();
            return true;
        }

        return false;
    }
    
    private void OnDestroy()
    {
        foreach (var keyValue in dictionaryGuidInstances)
        {
            keyValue.Value.stop(STOP_MODE.IMMEDIATE);
            keyValue.Value.release();
        }
    }
}
