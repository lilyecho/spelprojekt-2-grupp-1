using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "LevelHandling/Port/Registration")]
public class RegistrationPort : ScriptableObject
{
    public enum TypeOfRegistration
    {
        NonDefined,
        Player,
        Enemy,
        EnemyManager
    }
    
    public UnityAction<TypeOfRegistration, GameObject> OnRegisterAwake = delegate(TypeOfRegistration arg0, GameObject arg1) {  };
    public UnityAction<TypeOfRegistration, GameObject> OnRegisterStart = delegate(TypeOfRegistration arg0, GameObject arg1) {  };
    
}
