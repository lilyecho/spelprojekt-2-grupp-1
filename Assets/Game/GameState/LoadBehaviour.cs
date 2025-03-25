using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoadBehaviour : MonoBehaviour
{
    [SerializeField,Range(0.01f,2)] private float changeSceneCooldown = 1;
    private float _currentTime = 0;
    
    //TODO
    
    
    private void FixedUpdate()
    {
        if (_currentTime > 0)
        {
            _currentTime -= Time.fixedDeltaTime;
            if (_currentTime <= 0)
            {
                _currentTime = changeSceneCooldown;
            }
        }
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public static void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index >= SceneManager.sceneCountInBuildSettings) return;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    
    public static void LoadLastScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex - 1;
        if (index < 0) return;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        
    }

    public void ReloadScene(InputAction.CallbackContext context)
    {
        if (_currentTime > 0) return;
       
        if (context.performed)
        {
            ReloadScene();
        }
    }
    public void LoadLastScene(InputAction.CallbackContext context)
    {
        if (_currentTime > 0) return;
        
        if (context.performed)
        {
            LoadLastScene();
        }
    }
    public void LoadNextScene(InputAction.CallbackContext context)
    {
        if (_currentTime > 0) return;
        
        if (context.performed)
        {
            LoadNextScene();
        }
    }
    
    
    
    
}
