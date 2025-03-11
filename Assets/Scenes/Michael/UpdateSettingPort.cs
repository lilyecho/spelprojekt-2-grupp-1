using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UpdateSettingPort")]
public class UpdateSettingPort : ScriptableObject
{
    public UnityEvent updateSetting;
}
