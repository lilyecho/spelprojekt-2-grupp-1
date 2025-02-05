using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioTrigger : MonoBehaviour
{
    
    public UnityEvent onTrigger;
    
    public EventInfo[] info;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RuntimeManager.StudioSystem.update();
    }


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        AudioManager.Instance.InvokeEventInfo(info);
        onTrigger.Invoke();
        Debug.Log("Hit the trigger");
    }
}
