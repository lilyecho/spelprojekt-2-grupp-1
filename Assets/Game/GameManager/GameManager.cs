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
        Cursor.visible = true;
    }
    public void UnpauseGame()
    {
        unPause.Invoke();
        
        pauseMenu.gameObject.SetActive(false);
        timeManager.Movement(true);
        Cursor.visible = false;
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
    

    public string bellAlert = "Bell Alert";
    public string bellNormal = "Bell Normal";
    public string bellShaking = "Bell Shaking";
    public string hideBell = "Hide Bell";

    public void PlayBellAlert()
    {
        bellAnim.SetBool("Normal", false);
        bellAnim.SetBool("Shaking", false);
        bellAnim.SetBool("Invisible", false);
        bellAnim.SetBool("Alert", true);
        bellAnim.Play("Bell Alert");
        

        Debug.Log("ALERT");
    }
    public void PlayBellNormal()
    {
        bellAnim.SetBool("Alert", false);
        bellAnim.SetBool("Shaking", false);
        bellAnim.SetBool("Invisible", false);
        bellAnim.SetBool("Normal", true);
    }
    public void PlayBellShaking()
    {
        bellAnim.SetBool("Alert", false);
        bellAnim.SetBool("Normal", false);
        bellAnim.SetBool("Invisible", false);
        bellAnim.SetBool("Shaking", true);
    }

    public void PlayBellInvisible()
    {
        bellAnim.SetBool("Alert", false);
        bellAnim.SetBool("Normal", false);
        bellAnim.SetBool("Shaking", false);
        bellAnim.SetBool("Invisible", true);
    }
   

    

}
