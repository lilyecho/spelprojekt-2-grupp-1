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
        
        [Space,Header("Enemy-Related")]
        public SoundInfo[] onChased;
        public SoundInfo[] onNotChased;
        public SoundInfo[] onClose;
    }

    private void Start()
    {
        audioHandler.HandleSoundInfos(soundInfos.onAwake);
    }

    private void SceneChange(Scene scene, LoadSceneMode loadSceneMode)
    {
        
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
