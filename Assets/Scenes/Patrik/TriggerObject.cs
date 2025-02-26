using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(RuntimeAnimatorController))]
public class TriggerObject : MonoBehaviour
{
    [SerializeField] private TriggerQualities triggerQualities;

    [SerializeField] private TriggerActivityParameters OnEnter;
    [SerializeField] private TriggerActivityParameters OnStay;
    [SerializeField] private TriggerActivityParameters OnExit;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    [Flags]
    private enum TriggerQualities
    {
        Enter = 1,
        Stay = 2,
        Exit = 4
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!triggerQualities.HasFlag(TriggerQualities.Enter)) return;
        DoTriggerResults(OnEnter);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!triggerQualities.HasFlag(TriggerQualities.Stay)) return;
        DoTriggerResults(OnStay);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!triggerQualities.HasFlag(TriggerQualities.Exit)) return;
        DoTriggerResults(OnExit);
    }

    private void DoTriggerResults(TriggerActivityParameters activityParameters)
    {
        ActivateAnimation(activityParameters);
        ActivateParticles(activityParameters);
    }

    private void ActivateAnimation(TriggerActivityParameters activityParameters)
    {
        if (activityParameters.resultsFromTrigger.HasFlag(TriggerActivityParameters.TriggerResults.Particles) && !activityParameters.particleSystem.isPlaying)
        {
            activityParameters.particleSystem.Play();
        }
    }

    private void ActivateParticles(TriggerActivityParameters activityParameters)
    {
        if (activityParameters.resultsFromTrigger.HasFlag(TriggerActivityParameters.TriggerResults.Animation))
        {
            animator.Play(activityParameters.animationStateName);
        }
    }

    
    
    private void OnValidate()
    {
        Collider validateCollider = GetComponent<Collider>();
        if (!validateCollider.isTrigger)
        {
            Debug.Log(gameObject.name +" activates trigger quality in collider");
            validateCollider.isTrigger = true;
        }

        if (triggerQualities.HasFlag(TriggerQualities.Enter))
        {
            CheckAllActivityParameters(OnEnter);
        }

        if (triggerQualities.HasFlag(TriggerQualities.Stay))
        {
            CheckAllActivityParameters(OnStay);
        }

        if (triggerQualities.HasFlag(TriggerQualities.Exit))
        {
            CheckAllActivityParameters(OnExit);
        }

    }

    private void CheckAllActivityParameters(TriggerActivityParameters activityParameters)
    {
        //TODO check if stateName is defined
        
    }
    
}

[Serializable]
public struct TriggerActivityParameters
{
    public TriggerResults resultsFromTrigger;
    public ParticleSystem particleSystem;
    public string animationStateName;

    [Flags]
    public enum TriggerResults
    {
        Particles = 1,
        Animation =2
    }
}
