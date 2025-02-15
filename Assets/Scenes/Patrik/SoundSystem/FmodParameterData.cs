using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Audio/Fmod/FmodParameterData")]
public class FmodParameterData : ScriptableObject
{
    [Header("Global")]
    [SerializeField] private string closeToTrollParameter;
    [SerializeField] private string trollChasingParameter;
    [Header("Local")]
    [SerializeField] private string materialParameter;


    public string GetMaterialParameter => materialParameter;
}
