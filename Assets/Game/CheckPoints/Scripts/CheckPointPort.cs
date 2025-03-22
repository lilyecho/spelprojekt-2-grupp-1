using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "CheckPointPort")]
public class CheckPointPort : ScriptableObject
{
    public UnityAction<CheckPointBehaviour> OnChangeCheckPoint = delegate(CheckPointBehaviour checkPoint){ };
    public UnityAction OnRespawn = delegate(){};

    public void ChangeCheckPoint(CheckPointBehaviour checkPointBehaviour)
    {
        OnChangeCheckPoint.Invoke(checkPointBehaviour);
    }

    public void Respawn()
    {
        OnRespawn.Invoke();
    }
}
