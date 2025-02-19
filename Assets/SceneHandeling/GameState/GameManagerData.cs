using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "GameManagerData")]
public class GameManagerData : ScriptableObject
{
    [SerializeField] private LevelData[] levelDatas;
    private void Awake()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadData(levelDatas[sceneIndex]);
        
    }

    private void LoadData(LevelData levelData)
    {
        
    }
    
    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public static void ForwardScene()
    {
        try
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
        catch (Exception e)
        {
            Debug.LogError("No more scene to load");
            throw;
        }
        
    }
    
    public static void BackwardScene()
    {
        try
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        }
        catch (Exception e)
        {
            Debug.LogError("This current scene is the first to load");
            throw;
        }
        
    }
    
    
    
}
