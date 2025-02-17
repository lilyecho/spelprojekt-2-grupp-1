using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Switch : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        // gets the curent screen
        Scene sceneLoaded = SceneManager.GetActiveScene();
        // loads next level
        SceneManager.LoadScene(sceneLoaded.buildIndex + 1);
    }
}