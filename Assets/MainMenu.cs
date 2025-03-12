using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void RunOnCTRL()
    {
        GameManager.instance.RunOnCTRL();
    }

    public void RunOnShift()
    {
        GameManager.instance.RunOnShift();
    }

    public void UpdateCameraSense()
    {
        GameManager.instance.UpdateCameraSense();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
