using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundAlertCreation
{
    public static void CreateAlertPoint(SoundAlertInfo info, EnemyManagerPort enemyManagerPort)
    {
        enemyManagerPort.OnSoundAlert(info);
    }
}

[Serializable]
public struct SoundAlertInfo
{
    public Vector3 point;
    [Min(0)]public float soundRange;
}