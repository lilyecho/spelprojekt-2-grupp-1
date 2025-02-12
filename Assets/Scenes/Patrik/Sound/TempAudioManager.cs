using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAudioManager : MonoBehaviour
{
    [SerializeField] private RegistrationPort enemyManagerRegistrationPort;
    [SerializeField] private EnemyManager enemyManager;

    private void OnEnable()
    {
        throw new NotImplementedException();
    }

    private void OnDisable()
    {
        throw new NotImplementedException();
    }

    private void SetEnemyManager(RegistrationPort.TypeOfRegistration type, GameObject enemyManagerGameObject)
    {
        if (type != RegistrationPort.TypeOfRegistration.EnemyManager) return;

        enemyManager = enemyManagerGameObject.GetComponent<EnemyManager>();
    }
}
