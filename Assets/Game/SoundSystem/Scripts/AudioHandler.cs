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
using EventInstance = FMOD.Studio.EventInstance;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] private AudioPort audioPort = null;
    [SerializeField] private RegistrationPort registrationPort = null;
    [SerializeField] private Transform playerTransform = null;
    
    private Dictionary<GUID,EventInstance> dictionaryGuidSceneInstances = new Dictionary<GUID, EventInstance>();
    private Dictionary<GUID,EventInstance> dictionaryGuidGameInstances = new Dictionary<GUID, EventInstance>();
    
    [SerializeField] private bool debugMode;

    public AudioPort GetAudioPort => audioPort;
    
    private void OnEnable()
    {
        audioPort.OnSoundInfo += HandleSoundInfo;
        audioPort.OnSoundInfos += HandleSoundInfos;

        registrationPort.OnRegisterAwake += UpdatePlayerTransform;
        registrationPort.OnRegisterStart += UpdatePlayerTransform;
    }

    private void OnDisable()
    {
        audioPort.OnSoundInfo -= HandleSoundInfo;
        audioPort.OnSoundInfos -= HandleSoundInfos;
        
        registrationPort.OnRegisterAwake -= UpdatePlayerTransform;
        registrationPort.OnRegisterStart -= UpdatePlayerTransform;
    }

    private void Update()
    {
        if (debugMode)
        {
            string t = "Game instances: "+dictionaryGuidGameInstances.Count;
            foreach (EventInstance eventInstance in dictionaryGuidGameInstances.Values)
            {
                eventInstance.getDescription(out EventDescription eventDescription);
                eventDescription.getPath(out string path);
                t += "\n --> "+path;
            }
            
            t += "\n\nScene instances: "+dictionaryGuidSceneInstances.Count;
            foreach (EventInstance eventInstance in dictionaryGuidSceneInstances.Values)
            {
                eventInstance.getDescription(out EventDescription eventDescription);
                eventDescription.getPath(out string path);
                t += "\n --> "+path;
            }
            Debug.Log(t);
        }
    }

    private void UpdatePlayerTransform(RegistrationPort.TypeOfRegistration typeOfRegistration, GameObject newGameObject)
    {
        if (typeOfRegistration != RegistrationPort.TypeOfRegistration.Player) return;
        playerTransform = newGameObject.transform;
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
        
        HandleCreate(soundInfo);
        HandleParameterChange(soundInfo);
        HandleLocation(ref soundInfo);
        HandlePlay(soundInfo);
        HandleStop(soundInfo);
    }

    private bool HandleCreate(SoundInfo soundInfo)
    {
        if (!soundInfo.action.HasFlag(SoundInfo.SoundAction.Create)) return false;

        if (soundInfo.instanceVariant is SoundInfo.InstanceVariant.SceneInstance or SoundInfo.InstanceVariant.OneShot)
        {
            return CreateInstance(ref dictionaryGuidSceneInstances,soundInfo.eventReference);
        }
        if (soundInfo.instanceVariant == SoundInfo.InstanceVariant.GameInstance)
        {
            return CreateInstance(ref dictionaryGuidGameInstances,soundInfo.eventReference);
        }
        return false;
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
    
    private void HandleLocation(ref SoundInfo soundInfo)
    {
        if (!soundInfo.action.HasFlag(SoundInfo.SoundAction.Location)) return;
        
        //Handle null-input transform
        if (soundInfo.locationTransform == null && playerTransform == null)
        {
            if (debugMode) Debug.Log(soundInfo.soundImplementationName+": Missing transform from player in location-handling for sound");
            GameObject playerObject = GameObject.FindWithTag("Player");
            
            if (playerObject == null)
            {
                if (debugMode) Debug.Log(soundInfo.soundImplementationName+": Missing player in scene - Setting camera as transform");
                soundInfo.locationTransform = Camera.main.transform;
            }
            else
            {
                soundInfo.locationTransform = playerObject.transform;
            }
            
            if (soundInfo.locationTransform == null)
            {
                Debug.Log(soundInfo.soundImplementationName+": Missing camera-transform --> Wont make this sound without a valid transform");
                return;
            }
        }
        //Playertransform already declared
        else if (soundInfo.locationTransform == null)
        {
            if (debugMode) Debug.Log(soundInfo.soundImplementationName+": Missing inputTransform - Transform is now player");
            soundInfo.locationTransform = playerTransform;
        }
        //Valid locationTransform
        else
        {
            if (debugMode) Debug.Log(soundInfo.soundImplementationName+": ");
        }
        
        
        if (soundInfo.instanceVariant is SoundInfo.InstanceVariant.SceneInstance or SoundInfo.InstanceVariant.OneShot)
        {
            if (!TryGetInstance(ref dictionaryGuidSceneInstances,soundInfo.eventReference, out EventInstance instance)) return;
            
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
        instance.getPlaybackState(out PLAYBACK_STATE state);
        if (state == PLAYBACK_STATE.PLAYING) return;
        
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
    
    private bool CreateInstance(ref Dictionary<GUID, EventInstance> instanceDic, EventReference eventReference)
    {
        GUID eventGUID = eventReference.Guid;
        if (instanceDic.ContainsKey(eventGUID)) return false;
        
        instanceDic[eventReference.Guid] = RuntimeManager.CreateInstance(eventReference);
        return true;
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

        instance = instanceDict[eventGUID];
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

    public void ResetAllDicts()
    {
        EndAllInstances(ref dictionaryGuidSceneInstances);
        EndAllInstances(ref dictionaryGuidGameInstances);
    }
    
    private void EndAllInstances(ref Dictionary<GUID, EventInstance> dict)
    {
        foreach (var keyValue in dict)
        {
            keyValue.Value.stop(STOP_MODE.IMMEDIATE);
            keyValue.Value.release();
        }

        dict = new Dictionary<GUID, EventInstance>();
    }

    private void OnDestroy()
    {
        foreach (var keyValue in dictionaryGuidSceneInstances)
        {
            keyValue.Value.stop(STOP_MODE.IMMEDIATE);
            keyValue.Value.release();
        }
        
        foreach (var keyValue in dictionaryGuidGameInstances)
        {
            keyValue.Value.stop(STOP_MODE.IMMEDIATE);
            keyValue.Value.release();
        }
    }
}
