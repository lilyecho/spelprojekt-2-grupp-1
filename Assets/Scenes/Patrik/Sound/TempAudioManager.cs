using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAudioManager : MonoBehaviour
{
    [SerializeField] private RegistrationPort registrationPort;
    [SerializeField, ReadOnly] private EnemyManager enemyManager;
    //[SerializeField, ReadOnly] private Transform playerTransform;

    private void OnEnable()
    {
        registrationPort.OnRegister += SetRegistration;
    }

    private void OnDisable()
    {
        registrationPort.OnRegister -= SetRegistration;
    }

    private void SetRegistration(RegistrationPort.TypeOfRegistration type, GameObject enemyManagerGameObject)
    {
        switch (type)
        {
            case RegistrationPort.TypeOfRegistration.EnemyManager:
                enemyManager = enemyManagerGameObject.GetComponent<EnemyManager>();
                break;
            
            /*case RegistrationPort.TypeOfRegistration.Player:
                playerTransform = enemyManagerGameObject.GetComponent<Transform>();
                break;*/
            default:
                return;
        }
    }

    private void FixedUpdate()
    {
        float distance = enemyManager.GetClosestDistanceToEnemyFromPlayer();
    }
}
