using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class CheckPointBehaviour : MonoBehaviour
{
    [SerializeField] private RegistrationPort registrationPort = null;

    [SerializeField] private GameObject player;
    
    public UnityEvent changesOnStart;

    [SerializeField] private BoxCollider checkCollider = null;
    [SerializeField] private Color visualizerColor = Color.black;

    private void Awake()
    {
        registrationPort.OnRegister += Registration;
        
        checkCollider = GetComponent<BoxCollider>();
    }

    private void Registration(RegistrationPort.TypeOfRegistration typeOfRegistration, GameObject gameObject)
    {
        if (typeOfRegistration != RegistrationPort.TypeOfRegistration.Player) return;
        player = gameObject;
    }

    private void Start()
    {
        changesOnStart.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = visualizerColor;
        Gizmos.DrawWireCube(checkCollider.transform.position, checkCollider.size);
    }
}
