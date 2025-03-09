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
        Debug.Log("reload");
        if (context.canceled)
        {
            ReloadScene();
        }
    }
    public void LoadLastScene(InputAction.CallbackContext context)
    {
        Debug.Log("last");
        if (context.canceled)
        {
            LoadLastScene();
        }
    }
    public void LoadNextScene(InputAction.CallbackContext context)
    {
        Debug.Log("Next");
        if (context.canceled)
        {
            LoadNextScene();
        }
    }
    
    
    
    
}
