using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoadBehaviour : MonoBehaviour
{
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
        if (context.performed)
        {
            ReloadScene();
        }
    }
    public void LoadLastScene(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LoadLastScene();
        }
    }
    public void LoadNextScene(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LoadNextScene();
        }
    }
    
    
    
    
}
