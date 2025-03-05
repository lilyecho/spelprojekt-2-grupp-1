using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class CheckPointBehaviour : MonoBehaviour
{
    public UnityEvent changesOnStart;

    [SerializeField] private BoxCollider checkCollider = null;
    [SerializeField] private Color visualizerColor = Color.black;

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
