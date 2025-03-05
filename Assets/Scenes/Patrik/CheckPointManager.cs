using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CheckPointManager : MonoBehaviour
{
    [SerializeField] private CheckPointPort checkPointPort = null;
    [SerializeField,ReadOnly] private CheckPointBehaviour latestCheckPoint = null;
    [SerializeField, ReadOnly] private AbilityData.Abilities latestAbilities;
    private void Awake()
    {
        checkPointPort.OnChangeCheckPoint += ChangeLatestCheckPoint;
        //checkPointPort.OnChangeCheckPoint += ChangeLatestAbilities;
    }

    private void ChangeLatestCheckPoint(CheckPointBehaviour checkPoint)
    {
        latestCheckPoint = checkPoint;
    }
    private void ChangeLatestAbilities(AbilityData.Abilities currentAbilities)
    {
        latestAbilities = currentAbilities;
    }
    
    
}
