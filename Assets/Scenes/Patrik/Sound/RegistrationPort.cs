using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scene/Port/Registration")]
public class RegistrationPort : ScriptableObject
{
    public enum TypeOfRegistration
    {
        NonDefined,
        Player,
        Enemy,
        EnemyManager
    
    }
    
    public UnityAction<TypeOfRegistration, GameObject> OnRegister = delegate(TypeOfRegistration arg0, GameObject arg1) {  };
    
}
