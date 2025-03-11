using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {
        PauseGame();
    }

    



    public void PauseGame()
    {
        timeManager.Movement(false);
        pauseMenu.gameObject.SetActive(true);
        defaultButton.Select();
        
        
    }
    public void UnpauseGame()
    {
        pauseMenu.gameObject.SetActive(false);
        timeManager.Movement(true);
    }

    public void UpdateCameraSense()
    {
        cameraSensitivity = cameraSensitivitySlider.value;
    }
}
