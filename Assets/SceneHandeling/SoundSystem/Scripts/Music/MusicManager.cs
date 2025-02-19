using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MusicManager : MonoBehaviour
{
    [Header("Port-Related")]
    [SerializeField] private RegistrationPort registrationPort = null;
    [Header("Music-Related")]
    [SerializeField] private MusicData musicData;
    [SerializeField] private AudioHandler audioHandler = null;
    [Header("Behaviour-Related")]
    [SerializeField, ReadOnly] private EnemyManager enemyManager = null;
    [SerializeField] private float closeDistance;

    [Space, Header("MainMusic")] 
    [SerializeField] private UnityEvent test = new UnityEvent();
    
    
    //TODO väldigt temportärt endast för speltest 1
    private string parameterName = "";
    private int value;
    private void Awake()
    {
        //InitialSetup();
    }

    private void InitialSetup()
    {
        audioHandler.TryCreateInstance(musicData.GetMainMusic, out EventInstance mainInstance);
        mainInstance.start();
    }

    public void ChangeParameterName(string newParameter)
    {
        parameterName = newParameter;
    }

    public void ChangeValue(int newValue)
    {
        value = newValue;
    }

    public void CreateMusic()
    {
        audioHandler.TryCreateInstance(musicData.GetMainMusic);
    }

    public void ChangeLocalParameter()
    {
        audioHandler.TryChangeLocalParameter(musicData.GetMainMusic, parameterName, value);
    }
    
    public void ChangeGlobalParameter()
    {
        audioHandler.TryChangeGlobalParameter(parameterName, value);
    }

    public void StartMusic()
    {
        audioHandler.TryStartSound(musicData.GetMainMusic);
    }
    

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
