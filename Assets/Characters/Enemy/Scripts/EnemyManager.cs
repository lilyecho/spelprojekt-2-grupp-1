using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    
    //[SerializeField] private RegistrationPort registrationPort = null;
    [SerializeField] private EnemyManagerPort enemyManagerPort = null;
    [SerializeField] private List<EnemyBehaviour> enemies;

    [SerializeField] private int amountOfChaseUnits = 0;


    [Header("Audio")] 
    [SerializeField] private AudioPort audioPort = null; 
    private void Awake()
    {
        enemies = new List<EnemyBehaviour>();

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        enemyManagerPort.OnRegisterStart += RegisterEnemy;
        enemyManagerPort.OnChaseChange += UpdateChaseUnitValue;
        enemyManagerPort.OnSoundAlert += ActivateSearchForAlert;

        SceneManager.sceneLoaded += SceneChange;
    }
    private void OnDisable()
    {
        enemyManagerPort.OnRegisterStart -= RegisterEnemy;
        enemyManagerPort.OnChaseChange -= UpdateChaseUnitValue;
        enemyManagerPort.OnSoundAlert -= ActivateSearchForAlert;

        SceneManager.sceneLoaded -= SceneChange;
    }

    private void RegisterEnemy(RegistrationPort.TypeOfRegistration type,GameObject newEnemy)
    {
        if (type != RegistrationPort.TypeOfRegistration.Enemy) return;
        
        enemies.Add(newEnemy.GetComponent<EnemyBehaviour>());
        newEnemy.name = "Troll: "+enemies.Count;
    }

    private void ActivateSearchForAlert(SoundAlertInfo soundAlertInfo)
    {
        EnemyBehaviour[] alertedEnemies = FindAllEnemiesInRange(soundAlertInfo.point, soundAlertInfo.soundRange);

        foreach (EnemyBehaviour enemy in alertedEnemies)
        {
            enemy.Alerted(soundAlertInfo.point);
        }
    }
    
    
    private EnemyBehaviour[] FindAllEnemiesInRange(Vector3 pos, float range)
    {
        List<EnemyBehaviour> enemiesInRange = new List<EnemyBehaviour>();
        foreach (EnemyBehaviour enemy in enemies)
        {
            if (Vector3.Distance(pos,enemy.transform.position) < range)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange.ToArray();
    }
    
    public bool GetClosestDistanceToEnemyFromPlayer( out float? closestDistance)
    {
        closestDistance = null;
        foreach (var enemyBehaviour in enemies)
        {
            if (!enemyBehaviour.GetDistanceToPlayer(out float? distance)) continue;
            
            if (closestDistance == null || closestDistance > distance) closestDistance = distance;
        }

        if (closestDistance == null) return false;
        return true;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="changeAmount"></param>
    private void UpdateChaseUnitValue(ChangeValue change)
    {
        int preAmount = amountOfChaseUnits;
        amountOfChaseUnits += change == ChangeValue.Increase ? 1 : -1;
        CheckCurrentAmountForChase(preAmount);
    }

    private void CheckCurrentAmountForChase(int preAmount)
    {
        int diff = amountOfChaseUnits - preAmount;
        //Decrease from preAmount --> only check if zero
        if (diff < 0 && amountOfChaseUnits == 0)
        {
            audioPort.OnChased(false);
        }
        //Increase from preAmount --> only check if value is one
        else if (diff > 0 && amountOfChaseUnits == 1)
        {
            audioPort.OnChased(true);
        }
    }
    
    private void SceneChange(Scene scene, LoadSceneMode mode)
    {
        enemies = new List<EnemyBehaviour>();
        amountOfChaseUnits = 0;
    }
}
