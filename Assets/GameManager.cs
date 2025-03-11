using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject pauseMenu;
    public GameObject paused;
    public GameObject settings;
    public GameObject controls;


    public TimeManager timeManager;


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
        pauseMenu.gameObject.SetActive(true);
    }

    



    public void PauseGame()
    {
        timeManager.OnMovement(false);
        pauseMenu.gameObject.SetActive(true);
    }
    

}
