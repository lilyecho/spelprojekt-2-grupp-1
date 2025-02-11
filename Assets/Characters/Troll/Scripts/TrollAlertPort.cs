using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Troll/AlertPort")]
public class TrollAlertPort : ScriptableObject
{
    public UnityAction<Vector3> OnAlertedPosition = delegate(Vector3 arg0) { };
}
