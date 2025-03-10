using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] private AudioPort audioPort = null;
    [SerializeField] private FmodParameterData parameterData = null;
    private Dictionary<GUID,EventInstance> dictionaryGuidInstances = new Dictionary<GUID, EventInstance>();


    [SerializeField] private bool debugMode;
    
    
    
    private void OnEnable()
    {
        audioPort.OnSoundInfo += HandleSoundInfo;
        audioPort.OnSoundInfos += HandleSoundInfos;
        
        audioPort.OnChangeGlobalParameter += ChangeGlobalParameter;
        audioPort.OnCreate += CreateInstance;
        audioPort.OnStart += PlayInstance;
        audioPort.OnSetParameter += ChangeLocalParameter;
        audioPort.OnRemove += RemoveInstance;
    }

    private void OnDisable()
    {
        audioPort.OnSoundInfo -= HandleSoundInfo;
        audioPort.OnSoundInfos -= HandleSoundInfos;
        
        audioPort.OnChangeGlobalParameter -= ChangeGlobalParameter;
        audioPort.OnCreate -= CreateInstance;
        audioPort.OnStart -= PlayInstance;
        audioPort.OnSetParameter -= ChangeLocalParameter;
        audioPort.OnRemove -= RemoveInstance;
    }

    private void Update()
    {
        if (debugMode)
        {
            Debug.Log("Amount of instances: "+dictionaryGuidInstances.Count);
        }
    }

    public void HandleSoundInfos(SoundInfo[] soundInfos)
    {
        foreach (SoundInfo soundInfo in soundInfos)
        {
            HandleSoundInfo(soundInfo);
        }
    }
    
    public void HandleSoundInfo(SoundInfo soundInfo)
    {
        if (soundInfo.action == 0) return;
        
        //TODO depending on if existic instance or created one
        HandleCreate(soundInfo);
        HandleParameterChange(soundInfo);
        HandleLocation(soundInfo);
        HandlePlay(soundInfo);
    }

    private void HandleCreate(SoundInfo soundInfo)
    {
        if (!soundInfo.action.HasFlag(SoundInfo.SoundAction.Create)) return;
        
        CreateInstance(soundInfo.eventReference);
    }

    private void HandleParameterChange(SoundInfo soundInfo)
    {
        if (!soundInfo.action.HasFlag(SoundInfo.SoundAction.ChangeParameter)) return;
        if (!TryGetInstance(soundInfo.eventReference, out EventInstance instance)) return;
        
        if (soundInfo.locality.HasFlag(SoundInfo.SoundLocality.Global))
        {
            TryChangeGlobalParameter(soundInfo.parameterName, soundInfo.parameterValue);
        }
        else
        {
            NewTryChangeLocalParameter(instance, soundInfo.parameterName, soundInfo.parameterValue);
        }
    }
    
    private void NewTryChangeLocalParameter(EventInstance instance, string parameterName, float value)
    {
        instance.setParameterByName(parameterName, value);
    }
    
    private void HandleLocation(SoundInfo soundInfo)
    {
        if (!soundInfo.action.HasFlag(SoundInfo.SoundAction.Location)) return;
        if (!TryGetInstance(soundInfo.eventReference, out EventInstance instance)) return;
        
        if (soundInfo.locationVariant.HasFlag(SoundInfo.LocationVariant.Attached))
        {
            AttachInstanceToObject(instance, soundInfo.locationTransform);
        }
        else
        {
            PlaceInstanceOnPosition(instance, soundInfo.locationTransform.position);
        }
    }
    
    private void HandlePlay(SoundInfo soundInfo)
    {
        if (!soundInfo.action.HasFlag(SoundInfo.SoundAction.Play)) return;
        
        if (soundInfo.playVariant.HasFlag(SoundInfo.PlayVariant.OneShot))
        {
            NewPlayOneShot(soundInfo.eventReference);
        }
        else
        {
            PlayInstance(soundInfo.eventReference);
        }
    }
    
    private void NewPlayOneShot(EventReference eventReference)
    {
        if (!TryGetInstance(eventReference, out EventInstance instance)) return;
        instance.start();
        GUID eventGUID = eventReference.Guid;
        dictionaryGuidInstances.Remove(eventGUID);
        instance.release();
    }

    private void AttachInstanceToObject(EventInstance instance, Transform objectTransform)
    {
        RuntimeManager.AttachInstanceToGameObject(instance, objectTransform);
    }

    private void PlaceInstanceOnPosition(EventInstance instance, Vector3 placementPos)
    {
        instance.set3DAttributes(placementPos.To3DAttributes());
    }
    
    private void ChangeGlobalParameter(string parameterName, float value)
    {
        RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
    }
    
    private void CreateInstance(EventReference eventReference)
    {
        GUID eventGUID = eventReference.Guid;
        if (dictionaryGuidInstances.ContainsKey(eventGUID)) return;
        
        dictionaryGuidInstances[eventReference.Guid] = RuntimeManager.CreateInstance(eventReference);
    }

    private void PlayInstance(EventReference eventReference)
    {
        if (!TryGetInstance(eventReference, out EventInstance instance)) return;

        instance.start();
    }
    
    private void ChangeLocalParameter(EventReference reference, string parameterName, float value)
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
    public void PlayOneShot(EventReference eventReference, Vector3 placementPos, string parameterName, float parameterValue)
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
    public void PlayOneShot(EventReference eventReference, Vector3 placementPos, Dictionary<string, float> parameterNamesAndValues)
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

    public bool TryChangeLocalParameter(EventReference reference, string parameterName, float value)
    {
        if (dictionaryGuidInstances.ContainsKey(reference.Guid))
        {
            dictionaryGuidInstances[reference.Guid].setParameterByName(parameterName, value);
            return true;
        }
        return false;
    }
    
    public void TryChangeGlobalParameter(string parameterName, float value)
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
