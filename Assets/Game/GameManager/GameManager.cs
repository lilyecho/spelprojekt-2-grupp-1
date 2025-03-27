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
    Vector2 hidePos = new Vector2(-10000, -10000);
    Vector2 showPos = new Vector2(-750, 250);

    public string bellAlert = "Bell Alert";
    public string bellNormal = "Bell Normal";
    public string bellShaking = "Bell Shaking";
    public string hideBell = "Hide Bell";

    public void PlayBellAlert()
    {
        ShowBell();
        bellAnim.Play("Bell Alert");
    }
    public void PlayBellNormal()
    {
        ShowBell();
        bellAnim.Play("Bell Normal");
    }
    public void PlayBellShaking()
    {
        ShowBell();
        bellAnim.Play("Bell Shaking");
    }
    public void HideBell()
    {
        bellGO.transform.position = hidePos;
    }
    public void ShowBell()
    {
        bellGO.transform.localPosition = showPos;
    }

    public IEnumerator UpdateBellAnimation(string animation)
    {
        if (GetCurrentAnimationName() == bellAlert)
        {
            //yield return new WaitForSeconds(GetRemainingAnimationTime());
            bellAnim.Play(animation);
            yield return new WaitForSeconds(GetRemainingAnimationTime());
            HideBell();
        }
        else if (GetCurrentAnimationName() == bellAlert && animation == hideBell)
        {
            yield return new WaitForSeconds(GetRemainingAnimationTime());
            HideBell();
        }
        else
        {
            bellAnim.Play(animation);
        }
    }

    float GetRemainingAnimationTime()
    {
        if (bellAnim == null) return 0f;

        AnimatorStateInfo stateInfo = bellAnim.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = Mathf.Repeat(stateInfo.normalizedTime, 1f);
        return stateInfo.length * (1 - normalizedTime);
    }

    string GetCurrentAnimationName()
    {
        if (bellAnim == null)
        {
            return null;
        }
        return bellAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

}
