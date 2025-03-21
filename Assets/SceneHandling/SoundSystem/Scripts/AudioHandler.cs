using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using SceneHandling.SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] private AudioPort audioPort = null;
    [SerializeField] private FmodParameterData parameterData = null;
    private Dictionary<GUID,EventInstance> dictionaryGuidSceneInstances = new Dictionary<GUID, EventInstance>();
    private Dictionary<GUID,EventInstance> dictionaryGuidGameInstances = new Dictionary<GUID, EventInstance>();
    
    [SerializeField] private bool debugMode;

    private void OnEnable()
    {
        audioPort.OnSoundInfo += HandleSoundInfo;
        audioPort.OnSoundInfos += HandleSoundInfos;

        SceneManager.sceneLoaded += SceneChange;
    }

    private void OnDisable()
    {
        audioPort.OnSoundInfo -= HandleSoundInfo;
        audioPort.OnSoundInfos -= HandleSoundInfos;
        
        SceneManager.sceneLoaded -= SceneChange;
    }

    private void Update()
    {
        if (debugMode)
        {
            Debug.Log("Amount of instances: "+dictionaryGuidSceneInstances.Count);
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
        HandleStop(soundInfo);
    }

    private void HandleCreate(SoundInfo soundInfo)
    {
        if (!soundInfo.action.HasFlag(SoundInfo.SoundAction.Create)) return;

        if (soundInfo.instanceVariant is SoundInfo.InstanceVariant.SceneInstance or SoundInfo.InstanceVariant.OneShot)
        {
            CreateInstance(ref dictionaryGuidSceneInstances,soundInfo.eventReference);
        }
        else if (soundInfo.instanceVariant == SoundInfo.InstanceVariant.GameInstance)
        {
            CreateInstance(ref dictionaryGuidGameInstances,soundInfo.eventReference);
        }
        
    }

    private void HandleParameterChange(SoundInfo soundInfo)
    {
        if (!soundInfo.action.HasFlag(SoundInfo.SoundAction.ChangeParameter)) return;
        
        if (soundInfo.locality == SoundInfo.SoundLocality.Global)
        {
            TryChangeGlobalParameter(soundInfo.parameterName, soundInfo.parameterValue);
            return;
        }
        
        if (soundInfo.instanceVariant is SoundInfo.InstanceVariant.SceneInstance or SoundInfo.InstanceVariant.OneShot)
        {
            if (!TryGetInstance(ref dictionaryGuidSceneInstances,soundInfo.eventReference, out EventInstance instance)) return;
            TryChangeLocalParameter(instance, soundInfo.parameterName, soundInfo.parameterValue);
        }
        else if (soundInfo.instanceVariant == SoundInfo.InstanceVariant.GameInstance)
        {
            if (!TryGetInstance(ref dictionaryGuidGameInstances,soundInfo.eventReference, out EventInstance instance)) return;
            TryChangeLocalParameter(instance, soundInfo.parameterName, soundInfo.parameterValue);
        }
    }
    
    private void TryChangeLocalParameter(EventInstance instance, string parameterName, float value)
    {
        instance.setParameterByName(parameterName, value);
    }
    
    private void HandleLocation(SoundInfo soundInfo)
    {
        if (!soundInfo.action.HasFlag(SoundInfo.SoundAction.Location)) return;
        
        if (soundInfo.instanceVariant is SoundInfo.InstanceVariant.SceneInstance or SoundInfo.InstanceVariant.OneShot)
        {
            if (!TryGetInstance(ref dictionaryGuidSceneInstances,soundInfo.eventReference, out EventInstance instance)) return;
            TryChangeLocalParameter(instance, soundInfo.parameterName, soundInfo.parameterValue);
            
            if (soundInfo.locationVariant == SoundInfo.LocationVariant.Attached)
            {
                AttachInstanceToObject(instance, soundInfo.locationTransform);
            }
            else
            {
                PlaceInstanceOnPosition(instance, soundInfo.locationTransform.position);
            }
        }
        else if (soundInfo.instanceVariant == SoundInfo.InstanceVariant.GameInstance)
        {
            if (!TryGetInstance(ref dictionaryGuidGameInstances,soundInfo.eventReference, out EventInstance instance)) return;
            TryChangeLocalParameter(instance, soundInfo.parameterName, soundInfo.parameterValue);
            
            if (soundInfo.locationVariant == SoundInfo.LocationVariant.Attached)
            {
                AttachInstanceToObject(instance, soundInfo.locationTransform);
            }
            else
            {
                PlaceInstanceOnPosition(instance, soundInfo.locationTransform.position);
            }
        }
    }
    
    private void HandlePlay(SoundInfo soundInfo)
    {
        if (!soundInfo.action.HasFlag(SoundInfo.SoundAction.Play)) return;

        Dictionary<GUID, EventInstance> instanceDict;
        
        if (soundInfo.instanceVariant is SoundInfo.InstanceVariant.SceneInstance or SoundInfo.InstanceVariant.OneShot)
        {
            instanceDict = dictionaryGuidSceneInstances;
        }
        else if (soundInfo.instanceVariant == SoundInfo.InstanceVariant.GameInstance)
        {
            instanceDict = dictionaryGuidGameInstances;
        }
        else
        {
            throw new Exception("other cases in HandlePlay that didnt handle the situation");
        }
        
        //When to create and start at the same time
        if (soundInfo.action.HasFlag(SoundInfo.SoundAction.Create))
        {
            if (soundInfo.instanceVariant == SoundInfo.InstanceVariant.OneShot)
            {
                PlayOneShot(ref instanceDict,soundInfo.eventReference);
            }
            else
            {
                PlayInstance(ref instanceDict,soundInfo.eventReference);
            }
            return;
        }
        
        //When only to start sound
        if (soundInfo.instanceVariant == SoundInfo.InstanceVariant.OneShot)
        {
            PlayOneShot(ref instanceDict, soundInfo.eventReference);
        }
        else
        {
            PlayInstance(ref instanceDict, soundInfo.eventReference);
        }
    }

    private void HandleStop(SoundInfo soundInfo)
    {
        if (!soundInfo.action.HasFlag(SoundInfo.SoundAction.Stop)) return;
        
        Dictionary<GUID, EventInstance> instanceDict;
        
        if (soundInfo.instanceVariant is SoundInfo.InstanceVariant.SceneInstance or SoundInfo.InstanceVariant.OneShot)
        {
            instanceDict = dictionaryGuidSceneInstances;
        }
        else if (soundInfo.instanceVariant == SoundInfo.InstanceVariant.GameInstance)
        {
            instanceDict = dictionaryGuidGameInstances;
        }
        else
        {
            throw new Exception("other cases in HandlePlay that didnt handle the situation");
        }
        
        TryStopInstance(ref instanceDict, soundInfo);
    }

    private void TryStopInstance(ref Dictionary<GUID, EventInstance> instanceDict ,SoundInfo soundInfo)
    {
        if (!TryGetInstance(ref instanceDict,soundInfo.eventReference, out EventInstance instance)) return;
        
        GUID eventGUID = soundInfo.eventReference.Guid;
        instanceDict.Remove(eventGUID);
        
        if (soundInfo.stopMode != SoundInfo.StopMode.None) 
        {
            instance.stop(soundInfo.stopMode== SoundInfo.StopMode.Immediate ? STOP_MODE.IMMEDIATE : STOP_MODE.ALLOWFADEOUT);
        }
        
        instance.release();
    }
    
    private void PlayOneShot(ref Dictionary<GUID, EventInstance> instanceDic,EventReference eventReference)
    {
        if (!TryGetInstance(ref instanceDic, eventReference, out EventInstance instance)) return;
        instance.start();
        GUID eventGUID = eventReference.Guid;
        instanceDic.Remove(eventGUID);
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
    
    private void CreateInstance(ref Dictionary<GUID, EventInstance> instanceDic, EventReference eventReference)
    {
        GUID eventGUID = eventReference.Guid;
        if (instanceDic.ContainsKey(eventGUID)) return;
        
        instanceDic[eventReference.Guid] = RuntimeManager.CreateInstance(eventReference);
    }

    private void PlayInstance(ref Dictionary<GUID, EventInstance> instanceDict, EventReference eventReference)
    {
        if (!TryGetInstance(ref instanceDict,eventReference, out EventInstance instance)) return;

        instance.start();
    }
    
    private bool TryGetInstance(ref Dictionary<GUID, EventInstance> instanceDict, EventReference eventReference, out EventInstance instance)
    {
        GUID eventGUID = eventReference.Guid;
        instance = new EventInstance();
        if (!instanceDict.ContainsKey(eventGUID)) return false;

        instance = dictionaryGuidSceneInstances[eventGUID];
        return true;
    }
    
    public bool TryCreateInstance(EventReference eventReference)
    {
        GUID eventGUID = eventReference.Guid;
        if (dictionaryGuidSceneInstances.ContainsKey(eventGUID))
        {
            return false;
        }
        dictionaryGuidSceneInstances[eventReference.Guid] = RuntimeManager.CreateInstance(eventReference);
        return true;
    }
    
    public bool TryChangeLocalParameter(EventReference reference, string parameterName, float value)
    {
        if (dictionaryGuidSceneInstances.ContainsKey(reference.Guid))
        {
            dictionaryGuidSceneInstances[reference.Guid].setParameterByName(parameterName, value);
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
    
    public bool TryStartSound(EventReference eventReference)
    {
        GUID eventGUID = eventReference.Guid;
        if (dictionaryGuidSceneInstances.ContainsKey(eventGUID))
        {
            dictionaryGuidSceneInstances[eventGUID].start();
            return true;
        }

        return false;
    }

    private void SceneChange(Scene scene, LoadSceneMode loadSceneMode)
    {
        EndAllSceneInstances();
    }

    private void EndAllSceneInstances()
    {
        foreach (var keyValue in dictionaryGuidSceneInstances)
        {
            keyValue.Value.stop(STOP_MODE.IMMEDIATE);
            keyValue.Value.release();
        }

        dictionaryGuidSceneInstances = new Dictionary<GUID, EventInstance>();
    }
}
