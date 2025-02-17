using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Unity.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Port-Related")]
    [SerializeField] private RegistrationPort registrationPort = null;
    [SerializeField] private AudioManagerPort audioManagerPort = null;
    [Header("Music-Related")]
    [SerializeField] private MusicData musicData;
    [SerializeField] private AudioHandler audioHandler = null;
    [Header("Behaviour-Related")]
    [SerializeField, ReadOnly] private EnemyManager enemyManager = null;
    [SerializeField] private float closeDistance;
    
    private void Awake()
    {
        InitialSetup();
    }

    private void InitialSetup()
    {
        audioHandler.CreateInstance(musicData.GetMainMusic, out EventInstance mainInstance);
        mainInstance.start();
    }

    private void OnEnable()
    {
        registrationPort.OnRegister += SetRegistration;
        //audioManagerPort.OnChased += PlayerChasedMusic;
        
    }

    private void OnDisable()
    {
        registrationPort.OnRegister -= SetRegistration;
        //audioManagerPort.OnChased -= PlayerChasedMusic;
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
        //CheckEnemyRelatedMusic();
    }

    /*
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
    }*/
}
