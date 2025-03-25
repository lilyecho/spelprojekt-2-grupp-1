using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistrationBehaviour : MonoBehaviour
{
    [SerializeField] private RegistrationPort registrationPort = null;
    [SerializeField] private RegistrationPort.TypeOfRegistration typeOfRegistration;

    [SerializeField] private bool registrationAwake;
    [SerializeField] private bool registrationStart;
    // Start is called before the first frame update
    
    private void Awake()
    {
        if (!registrationAwake) return;
        registrationPort.OnRegisterAwake(typeOfRegistration,gameObject);
        Debug.Log("Awake - player");
    }
    void Start()
    {
        if (!registrationStart) return;
        registrationPort.OnRegisterStart(typeOfRegistration,gameObject);
    }

    
}
