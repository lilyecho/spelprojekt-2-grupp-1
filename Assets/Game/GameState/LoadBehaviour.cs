using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
public class LoadBehaviour : MonoBehaviour
{
    [SerializeField,Range(0.01f,2)] private float changeSceneCooldown = 1;
    private float _currentTime = 0;

    private bool _respawnAvailable = true;
    private void FixedUpdate()
    {
        
        if (!_respawnAvailable)
        {
            _currentTime -= Time.fixedDeltaTime;
            if (_currentTime <= 0)
            {
                _respawnAvailable = true;
                _currentTime = changeSceneCooldown;
            }
        }
    }

    public static void ReloadScene()
    {
        Debug.Log("Static - Reload!");
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
        if (!_respawnAvailable) return;
       
        if (context.performed)
        {
            Debug.Log("Normal - Reload!!");
            _respawnAvailable = false;
            ReloadScene();
        }
    }
    public void LoadLastScene(InputAction.CallbackContext context)
    {
        if (!_respawnAvailable) return;
        
        if (context.performed)
        {
            _respawnAvailable = false;
            LoadLastScene();
        }
    }
    public void LoadNextScene(InputAction.CallbackContext context)
    {
        if (!_respawnAvailable) return;
        
        if (context.performed)
        {
            _respawnAvailable = false;
            LoadNextScene();
        }
    }
    
    
    
    
}
