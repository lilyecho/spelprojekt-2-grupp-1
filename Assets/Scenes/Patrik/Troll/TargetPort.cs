using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Troll/TargetPort")]
public class TargetPort : ScriptableObject
{
    public UnityAction<GameObject> OnTargetCreated = delegate(GameObject arg0) {  };
}
