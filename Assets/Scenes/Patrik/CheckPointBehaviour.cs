using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class CheckPointBehaviour : MonoBehaviour
{
    [SerializeField, ReadOnly] private AbilityData.Abilities savedAbilities; 
    
    public UnityEvent changesOnStart;

    [SerializeField] private BoxCollider checkCollider = null;
    [SerializeField] private Color visualizerColor = Color.black;

    public AbilityData.Abilities Abilities
    {
        get => savedAbilities;
        set => savedAbilities = value;
    }

    private void Awake()
    {
        
        
        checkCollider = GetComponent<BoxCollider>();
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
