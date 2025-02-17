using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{
    //[SerializeField] private RegistrationPort registrationPort = null;
    [SerializeField] private EnemyManagerPort enemyManagerPort = null;
    [SerializeField, ReadOnly] private List<EnemyBehaviour> enemies;

    [SerializeField,ReadOnly] private int amountOfChaseUnits = 0;


    [Header("Audio")] 
    [SerializeField] private AudioManagerPort audioManagerPort = null; 
    private void Awake()
    {
        enemies = new List<EnemyBehaviour>();
    }

    private void OnEnable()
    {
        enemyManagerPort.OnRegister += RegisterEnemy;
        enemyManagerPort.OnChaseChange += UpdateChaseUnitValue;
    }
    private void OnDisable()
    {
        enemyManagerPort.OnRegister -= RegisterEnemy;
        enemyManagerPort.OnChaseChange -= UpdateChaseUnitValue;
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
        
        return enemies[0].GetDistanceToPlayer();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="changeAmount"></param>
    private void UpdateChaseUnitValue(ChangeValue change)
    {
        int preAmount = amountOfChaseUnits;
        amountOfChaseUnits += change == ChangeValue.Increase ? 1 : -1;
        CheckCurrentAmountForChase(preAmount);
    }

    private void CheckCurrentAmountForChase(int preAmount)
    {
        int diff = amountOfChaseUnits - preAmount;
        //Decrease from preAmount --> only check if zero
        if (diff < 0 && amountOfChaseUnits == 0)
        {
            audioManagerPort.OnChased(false);
        }
        //Increase from preAmount --> only check if value is one
        else if (diff > 0 && amountOfChaseUnits == 1)
        {
            Debug.Log("Yes");
            audioManagerPort.OnChased(true);
        }
    }
    
    
    
}
