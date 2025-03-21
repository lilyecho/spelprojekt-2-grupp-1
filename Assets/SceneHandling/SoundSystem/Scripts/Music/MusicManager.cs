using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using SceneHandling.SoundSystem.Scripts;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [Header("Port-Related")]
    [SerializeField] private RegistrationPort registrationPort = null;
    [Header("Music-Related")]
    [SerializeField] private AudioHandler audioHandler = null;
    [Header("Behaviour-Related")]
    [SerializeField, ReadOnly] private EnemyManager enemyManager = null;
    [SerializeField] private float closeDistance;

    [SerializeField] private SoundInfos soundInfos;


    [Serializable]
    struct SoundInfos
    {
        [Header("MainMusic")] 
        public SoundInfo[] onMusic;
        
        [Space,Header("Enemy-Related")]
        public SoundInfo[] onChased;
        public SoundInfo[] onNotChased;
        public SoundInfo[] onClose;
    }

    private void Awake()
    {
        CreateMusic();
    }

    private void SceneChange(Scene scene, LoadSceneMode loadSceneMode)
    {
        
    }
    
    private void CreateMusic()
    {
        audioHandler.HandleSoundInfos(soundInfos.onMusic);
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
        if (enemyManager == null) return;
        if (!enemyManager.GetClosestDistanceToEnemyFromPlayer(out float? possibleDistance)) return;
        
        float distance = (float)possibleDistance;
        
        if (distance <= closeDistance)
        {
            float interpolationValue = MathF.Abs(distance / closeDistance-1);
            audioHandler.TryChangeGlobalParameter("CloseToTroll", interpolationValue);
        }
        else
        {
            audioHandler.TryChangeGlobalParameter("CloseToTroll", 0);
        }
        
    }

    private void OnValidate()
    {
        if (enemyManager == null)
        {
            Debug.LogWarning("Missing enemyManager in musicManager");
        }
    }
}
