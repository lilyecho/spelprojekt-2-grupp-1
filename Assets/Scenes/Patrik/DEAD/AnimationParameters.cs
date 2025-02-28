using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class AnimationParameters : ScriptableObject
{
    [SerializeField] private AnimationParametersCharacters[] parameters;
    
    public enum Parameters
    {
        Speed,
        Grounded,
        Sneaking,
        Running,
        Default
    }
    private void OnValidate()
    {
        foreach (AnimationParametersCharacters parameter in parameters)
        {
            if (Array.FindAll(parameters, x => x.parameter == parameter.parameter).Length > 1)
            {
                throw new DataException("Invalid parameter, each parameter is reserved for only one-time use and not to multiple parameters. If more is wanted add into the enum Parameters");
            }
            if (Array.FindAll(parameters, x => x.parameterName == parameter.parameterName).Length > 1)
            {
                throw new DataException("Invalid parameterName, each parameter is reserved for only one-time use and not to multiple parameters. Change name");
            }
            if (parameter.parameterName == "" || parameter.parameterName.Length == 0)
            {
                throw new DataException("Invalid data, each parameterName has to have a name for the parameter");
            }
        }
    }
}

[Serializable]
public struct AnimationParametersCharacters
{
    public AnimationParameters.Parameters parameter;
    public string parameterName;
}
