using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRelocation : MonoBehaviour
{
    [SerializeField] private Transform joint1;
    [SerializeField] private Transform joint2;
    [SerializeField, Range(0,1)] private float offset;

    private void FixedUpdate()
    {
        transform.position = CalculateNewPos();
    }

    private Vector3 CalculateNewPos()
    {
        Vector3 direction = (joint2.position - joint1.position).normalized;
        return  joint1.position + direction * offset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(CalculateNewPos(),new Vector3(.1f,.1f,.1f));
    }
}
