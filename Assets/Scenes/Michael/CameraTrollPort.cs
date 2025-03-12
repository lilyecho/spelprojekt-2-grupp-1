using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CameraTrollPort")]
public class CameraTrollPort : ScriptableObject
{
    public UnityAction< CameraTrollPort, Transform, Transform> OnAstridGettingCaught;

    public void AstridCaught(Transform troll, Transform cameraPos)
    {
        OnAstridGettingCaught?.Invoke(this, troll, cameraPos);
    }
}
