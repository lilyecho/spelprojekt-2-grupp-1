using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class GeneralTriggerBehaviour : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onStay;
    [SerializeField] private UnityEvent onExit;

    private List<GameObject> activeUsers = new List<GameObject>();
    
    private void OnValidate()
    {
        Collider validateCollider = GetComponent<Collider>();
        if (!validateCollider.isTrigger)
        {
            Debug.Log(gameObject.name +" activates trigger quality in collider");
            validateCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        onEnter.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        onStay.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
       
        onExit.Invoke();
    }
}

