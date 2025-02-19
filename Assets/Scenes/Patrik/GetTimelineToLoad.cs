using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

public class GetTimelineToLoad : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    
    private void FixedUpdate()
    {
        if (videoPlayer.isPaused)
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        LoadBehaviour.LoadNextScene(); 
    }
}
