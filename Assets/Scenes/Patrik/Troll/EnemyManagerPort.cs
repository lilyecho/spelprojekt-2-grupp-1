using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "LevelHandling/Port/EnemyManager")]
public class EnemyManagerPort : RegistrationPort
{
    public UnityAction<ChangeValue> OnChaseChange = delegate {};
}
