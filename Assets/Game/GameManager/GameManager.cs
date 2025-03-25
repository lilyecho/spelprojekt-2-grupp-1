using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject pauseMenu;
    public GameObject paused;
    public GameObject settings;
    public GameObject controls;
    public Button defaultButton;

    public TimeManager timeManager;


    public float cameraSensitivity = 1;
    public Slider cameraSensitivitySlider;

    public UnityEvent onPause;
    public UnityEvent unPause;
    
    public bool runOnCTRL = true;
    
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void PauseGame()
    {
        onPause.Invoke();
        
        timeManager.Movement(false);
        pauseMenu.gameObject.SetActive(true);
        
        
    }
    public void UnpauseGame()
    {
        unPause.Invoke();
        
        pauseMenu.gameObject.SetActive(false);
        timeManager.Movement(true);
    }

    public void UpdateCameraSense()
    {
        cameraSensitivity = cameraSensitivitySlider.value;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RunOnCTRL()
    {
        runOnCTRL = true;
    }

    public void RunOnShift()
    {
        runOnCTRL = false;
    }







    public Image bell;
    public GameObject bellGO;
    public Animator bellAnim;
    Vector2 hidePos = new Vector2(-10000, -10000);
    Vector2 showPos = new Vector2(-750, 250);

    public void PlayBellAlert()
    {
        bellGO.transform.position = showPos;
        bellAnim.Play("Bell Alert");
    }
    public void PlayBellNormal()
    {
        bellGO.transform.position = showPos;
        bellAnim.Play("Bell Normal");
    }
    public void PlayBellShaking()
    {
        bellGO.transform.position = showPos;
        bellAnim.Play("Bell Shaking");
    }
    public void HideBell()
    {
        bellGO.transform.position = hidePos;
    }


}
