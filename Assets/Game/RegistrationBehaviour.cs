using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistrationBehaviour : MonoBehaviour
{
    [SerializeField] private RegistrationPort registrationPort = null;
    [SerializeField] private RegistrationPort.TypeOfRegistration typeOfRegistration;
    
    // Start is called before the first frame update
    void Start()
    {
        registrationPort.OnRegister(typeOfRegistration,gameObject);
    }
}
