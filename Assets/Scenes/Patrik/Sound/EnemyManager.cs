using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private RegistrationPort registrationPort;
    [SerializeField, ReadOnly] private List<GameObject> enemies;

    private void Awake()
    {
        enemies = new List<GameObject>();
    }

    private void OnEnable()
    {
        registrationPort.OnRegister += RegisterEnemy;
    }
    private void OnDisable()
    {
        registrationPort.OnRegister -= RegisterEnemy;
    }

    private void RegisterEnemy(RegistrationPort.TypeOfRegistration type,GameObject newEnemy)
    {
        if (type != RegistrationPort.TypeOfRegistration.Enemy) return;
        
        enemies.Add(newEnemy);
        newEnemy.name = "Troll: "+enemies.Count;
    }
    
    
}
