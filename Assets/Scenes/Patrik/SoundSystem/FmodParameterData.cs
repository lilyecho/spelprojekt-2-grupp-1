using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Audio/Fmod/FmodParameterData")]
public class FmodParameterData : ScriptableObject
{
    [Header("Global")]
    [SerializeField] private string closeToTrollParameter;
    [SerializeField] private string trollChasingParameter;
    [SerializeField] private string weaIntensity;
    
    [Header("Local")]
    [SerializeField] private string materialParameter;
    [SerializeField] private string catLoopingVo;
    [SerializeField] private string catPowers;
    [SerializeField] private string isPowerActive;
    [SerializeField] private string catSneak;
    [SerializeField] private string elevation;
    [SerializeField] private string vegetation;
    

    public string GetMaterialParameter => materialParameter;
    
    public string GetCatLoopingVo => catLoopingVo;
    public string GetCatPowers => catPowers;
    public string GetIsPowerActive => isPowerActive;
    public string GetCatSneak => catSneak;
    public string GetElevation => elevation;
    public string GetVegetation => vegetation;

}
