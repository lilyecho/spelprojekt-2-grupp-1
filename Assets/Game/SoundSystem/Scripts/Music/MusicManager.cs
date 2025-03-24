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
    
    #region Singleton

    public static MusicManager instance;

    private void SingletonStructureCheck()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion
    
    private void Awake()
    {
        SingletonStructureCheck();
    }


    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SceneChange;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= SceneChange;
    }
    
    private void SceneChange(Scene preScene, Scene newScene)
    {
        UpdateMusicAccordingToScene();
    }

    private void UpdateMusicAccordingToScene()
    {
        
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                audioHandler.HandleSoundInfos(soundInfos.onAwake);
                Debug.Log("0");
                break;
            case 1:
                audioHandler.HandleSoundInfos(soundInfos.onLvl1);
                Debug.Log("1");
                break;
            case 2:
                audioHandler.HandleSoundInfos(soundInfos.onLvl2);
                Debug.Log("2");
                break;
            case 3:
                audioHandler.HandleSoundInfos(soundInfos.onLvl3);
                Debug.Log("3");
                break;
            case 4:
                audioHandler.HandleSoundInfos(soundInfos.onLvl4);
                Debug.Log("4");
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
