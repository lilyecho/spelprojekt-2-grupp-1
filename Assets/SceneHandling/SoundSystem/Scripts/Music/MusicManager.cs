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
    [Header("Music-Related")]
    [SerializeField] private AudioHandler audioHandler = null;
    [Header("Behaviour-Related")]
    [SerializeField] private EnemyManager enemyManager = null;
    [SerializeField] private float closeDistance;

    [SerializeField] private SoundInfos soundInfos;


    [Serializable]
    struct SoundInfos
    {
        [Header("MainMusic")] 
        public SoundInfo[] onAwake;

        public SoundInfo[] onLvl1;
        public SoundInfo[] onLvl2;
        public SoundInfo[] onLvl3;
        public SoundInfo[] onLvl4;
        
        [Space,Header("Enemy-Related")]
        public SoundInfo[] onChased;
        public SoundInfo[] onNotChased;
        public SoundInfo[] onClose;
    }

    private void Start()
    {
        Debug.LogError(transform.parent.parent.name);
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                audioHandler.HandleSoundInfos(soundInfos.onAwake);
                break;
            case 1:
                audioHandler.HandleSoundInfos(soundInfos.onLvl1);
                break;
            case 2:
                audioHandler.HandleSoundInfos(soundInfos.onLvl2);
                break;
            case 3:
                audioHandler.HandleSoundInfos(soundInfos.onLvl3);
                break;
            case 4:
                audioHandler.HandleSoundInfos(soundInfos.onLvl4);
                break;
            default:
                Debug.LogError("Missing sound-implementation for this lvl");
                break;
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
