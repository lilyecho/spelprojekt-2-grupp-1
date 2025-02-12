using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private RegistrationPort registrationPort;
    [SerializeField, ReadOnly] private List<EnemyBehaviour> enemies;

    private void Awake()
    {
        enemies = new List<EnemyBehaviour>();
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
        
        enemies.Add(newEnemy.GetComponent<EnemyBehaviour>());
        newEnemy.name = "Troll: "+enemies.Count;
    }


    public float GetClosestDistanceToEnemyFromPlayer()
    {
        enemies.Sort();

        string t = "";
        foreach (var enemy in enemies)
        {
            t += enemy.gameObject.name + " : " + enemy.GetDistanceToPlayer();
        }
        Debug.Log(t);
        return enemies[0].GetDistanceToPlayer();
    }
    
}
