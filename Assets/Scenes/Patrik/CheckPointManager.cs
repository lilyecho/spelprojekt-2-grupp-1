using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CheckPointManager : MonoBehaviour
{
    [SerializeField] private CheckPointPort checkPointPort = null;
    [SerializeField,ReadOnly] private CheckPointBehaviour latestCheckPoint = null;
    
    private void Awake()
    {
        checkPointPort.OnChangeCheckPoint += ChangeLatestCheckPoint;
    }

    private void ChangeLatestCheckPoint(CheckPointBehaviour checkPoint)
    {
        latestCheckPoint = checkPoint;
    }
    
    
}
