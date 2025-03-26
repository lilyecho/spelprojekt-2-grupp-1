using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SoundInfo = SceneHandling.SoundSystem.Scripts.SoundInfo;

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
        public SoundInfo[] onStartMenu;
        public SoundInfo[] onIntro;
        public SoundInfo[] onLvl1;
        public SoundInfo[] onLvl2;
        public SoundInfo[] onLvl3;
        public SoundInfo[] onLvl4;
        public SoundInfo[] onOutro;
        
        [Space,Header("Enemy-Related")]
        public SoundInfo[] onChased;
        public SoundInfo[] onNotChased;
        public SoundInfo[] onClose;
        public SoundInfo[] onNotClose;
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
        audioHandler.GetAudioPort.OnChased += Chased;
        SceneManager.activeSceneChanged += SceneChange;
    }

    private void OnDisable()
    {
        audioHandler.GetAudioPort.OnChased -= Chased;
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
                audioHandler.ResetAllDicts();
                audioHandler.HandleSoundInfos(soundInfos.onStartMenu);
                break;
            case 1:
                audioHandler.HandleSoundInfos(soundInfos.onIntro);
                break;
            case 2:
                audioHandler.HandleSoundInfos(soundInfos.onLvl1);
                break;
            case 3:
                audioHandler.HandleSoundInfos(soundInfos.onLvl2);
                break;
            case 4:
                audioHandler.HandleSoundInfos(soundInfos.onLvl3);
                break;
            case 5:
                audioHandler.HandleSoundInfos(soundInfos.onLvl4);
                break;
            case 6:
                audioHandler.HandleSoundInfos(soundInfos.onOutro);
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

        audioHandler.HandleSoundInfos(possibleDistance <= closeDistance ? soundInfos.onClose : soundInfos.onNotClose);
    }

    private void OnValidate()
    {
        if (enemyManager == null)
        {
            Debug.LogWarning("Missing enemyManager in musicManager");
        }
        
        CheckSoundInfos(ref soundInfos.onStartMenu);
        CheckSoundInfos(ref soundInfos.onIntro);
        CheckSoundInfos(ref soundInfos.onLvl1);
        CheckSoundInfos(ref soundInfos.onLvl2);
        CheckSoundInfos(ref soundInfos.onLvl3);
        CheckSoundInfos(ref soundInfos.onLvl4);
        CheckSoundInfos(ref soundInfos.onOutro);
    }

    private void CheckSoundInfos(ref SoundInfo[] soundInfos)
    {
        for (int i = 0; i < soundInfos.Length; i++)
        {
            SoundInfo controlSoundInfo = soundInfos[i];
            controlSoundInfo.locationTransform = null;
            soundInfos[i] = controlSoundInfo;
        }
    }

    private void Chased(bool isChased)
    {
        audioHandler.HandleSoundInfos(isChased ? soundInfos.onChased : soundInfos.onNotChased);
    }

}
