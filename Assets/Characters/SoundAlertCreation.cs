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

public struct SoundAlertInfo
{
    public Vector3 Point;
    [Min(0)]public float SoundRange;
}