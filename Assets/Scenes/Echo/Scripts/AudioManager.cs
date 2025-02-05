using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : MonoBehaviour
{

    
    public static AudioManager Instance;
    
    private Dictionary<GUID, EventInstance> eventDict;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this) // singleton
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
        DontDestroyOnLoad(this);
        
        
        eventDict = new Dictionary<GUID, EventInstance>();
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlayEvent(EventInfo eventInfo)
    {
        //if the event exists in the dictionary then don't play it
        
        GUID eventID = eventInfo.eventReference.Guid;
        if (eventDict.ContainsKey(eventID))
        {
            Debug.Log("event instance already exists: " + eventID);
            return;
        }

        EventInstance instance = RuntimeManager.CreateInstance(eventInfo.eventReference);
        
        if (eventInfo.attachedInstance && eventInfo.audioSource != null)
        {
            RuntimeManager.AttachInstanceToGameObject(instance, eventInfo.audioSource.transform);
        }
        instance.start();
        eventDict[eventInfo.eventReference.Guid] = instance;
        Debug.Log("Play Event");
    }
    
    public void StopEvent(EventInfo eventInfo)
    {
        GUID eventID = eventInfo.eventReference.Guid;
        if (!eventDict.TryGetValue(eventID, out EventInstance instance))
        {
            Debug.Log("event instance doesn't exist: " + eventID);
            return;
        }

        instance.stop(eventInfo.stopMode);
        instance.release();
        eventDict.Remove(eventID);
    }

    public void SetParameter(EventInfo eventInfo)
    {
            GUID eventID = eventInfo.eventReference.Guid;
            eventDict.TryGetValue(eventID, out EventInstance instance);

            if (instance.isValid())
            {
                instance.getDescription(out EventDescription eventDesc);
                eventDesc.getParameterDescriptionByName(eventInfo.paramName,
                    out PARAMETER_DESCRIPTION parameterDescription);
                bool global = parameterDescription.flags.HasFlag(PARAMETER_FLAGS.GLOBAL);

                if (global)
                {
                    RuntimeManager.StudioSystem.setParameterByName(eventInfo.paramName, eventInfo.value);
                }
                else
                {
                    instance.setParameterByName(eventInfo.paramName, eventInfo.value);
                }
            }
            else
            {
                try
                {
                    RuntimeManager.StudioSystem.setParameterByName(eventInfo.paramName, eventInfo.value);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Can't set {eventInfo.paramName} parameter: {e}");
                }
            }
    }
    
    
    public void InvokeEventInfo(EventInfo eventInfo)
    {
        switch (eventInfo.action)
        {
            case Action.Play:

                PlayEvent(eventInfo);

                break;
                
            case Action.Stop:
                    
                StopEvent(eventInfo);
                    
                break;
                
            case Action.SetParameter:

                SetParameter(eventInfo);

                break;
        }
    }
    
    public void InvokeEventInfo(EventInfo[] audioInfo)
    {
        foreach (EventInfo i in audioInfo)
        {
            InvokeEventInfo(i);
        }
    }
    
}
