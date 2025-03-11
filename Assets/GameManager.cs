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






    public static GameManager _Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singletonObject = new GameObject("GameManager");
                instance = singletonObject.AddComponent<GameManager>();

                DontDestroyOnLoad(singletonObject);
            }
            return instance;
        }
    }


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
        
    }

    



    public void PauseGame()
    {
        onPause.Invoke();
        
        timeManager.Movement(false);
        pauseMenu.gameObject.SetActive(true);
        defaultButton.Select();
        
        
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
}
