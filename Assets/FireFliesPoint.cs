using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FireFliesPoint : MonoBehaviour
{
    public bool isStop;

    public float distanceToTrigger;

    public float changeSpeedTo;

    [HideInInspector] public int index;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.2f);
        
    }


}
