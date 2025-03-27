using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GetTimelineToLoad : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private string sceneToLoad;

    //private bool isSceneChanging = false;

    private void FixedUpdate()
    {
        Debug.Log(videoPlayer.isPaused);
        if (videoPlayer.isPaused)
        {
            Debug.Log("ok");
            ChangeScene();
        }
    }

    private void ChangeScene()
    {

        SceneManager.LoadScene(sceneToLoad);
    }
}
