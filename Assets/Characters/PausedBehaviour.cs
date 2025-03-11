using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PausedBehaviour : MonoBehaviour
{
    [SerializeField] private TimeManager timeManager = null;
    
    private Rigidbody rb;
    private Animator animator;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        timeManager.OnMovement += ChangeMovementActivation;
    }

    private void OnDisable()
    {
        timeManager.OnMovement -= ChangeMovementActivation;
    }
    
    private void ChangeMovementActivation(bool nextValue)
    {
        //Fungerar som en stopper
        rb.constraints = nextValue ? RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        animator.speed = nextValue ? 1 : 0;
    }
}
