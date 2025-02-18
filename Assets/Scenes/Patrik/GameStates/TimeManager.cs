using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "LevelHandling/TimeManager")]
public class TimeManager : ScriptableObject
{
    public UnityAction<bool> OnMovement = delegate(bool s) { };
    
    public void Movement(bool isMovementOn)
    {
        OnMovement.Invoke(isMovementOn);
    }
    
}
