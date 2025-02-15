using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAudioManager : MonoBehaviour
{
    [SerializeField] private RegistrationPort registrationPort = null;
    [SerializeField] private AudioManagerPort audioManagerPort = null;
    [SerializeField, ReadOnly] private EnemyManager enemyManager = null;
    //[SerializeField, ReadOnly] private Transform playerTransform;

    [SerializeField] private EventInfo mainMusicEvent;

    [SerializeField] private float closeDistance;

    [SerializeField] private EventInfo CloseToTroll;
    [SerializeField] private EventInfo NotCloseToTroll;
    [SerializeField] private EventInfo Chasing;
    [SerializeField] private EventInfo NotChasing;
    
    private void Awake()
    {
        AudioManager.Instance.InvokeEventInfo(mainMusicEvent);
    }

    private void OnEnable()
    {
        registrationPort.OnRegister += SetRegistration;
        audioManagerPort.OnChased += PlayerChasedMusic;
        
    }

    private void OnDisable()
    {
        registrationPort.OnRegister -= SetRegistration;
        audioManagerPort.OnChased -= PlayerChasedMusic;
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
        CheckEnemyRelatedMusic();
    }

    private void CheckEnemyRelatedMusic()
    {
        float distance = enemyManager.GetClosestDistanceToEnemyFromPlayer();

        if (distance <= closeDistance)
        {
            AudioManager.Instance.InvokeEventInfo(CloseToTroll);
        }
        else
        {
            AudioManager.Instance.InvokeEventInfo(NotCloseToTroll);
        }
        
    }

    private void PlayerChasedMusic(bool onOff)
    {
        if (onOff)
        {
            AudioManager.Instance.InvokeEventInfo(Chasing);
        }
        else
        {
            AudioManager.Instance.InvokeEventInfo(NotChasing);
        }
        
    }
}
