using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "CameraPort")]
public class CameraPort : ScriptableObject
{
    public UnityAction<Transform> OnTarget = delegate(Transform target) {};
}
