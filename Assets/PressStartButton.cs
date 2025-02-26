using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressStartButton : MonoBehaviour
{

    public Animator animator;
    public GameObject image;
    
    public void PressStart()
    {
        image.SetActive(true);
        animator.SetBool("HasStarted", true);
        
        StartCoroutine(Wait());

    }
    
    IEnumerator Wait()
    {
        // do something before
        Debug.Log("Before");

        // waits here
        yield return new WaitForSeconds(2f);
        
        LoadBehaviour.LoadNextScene();

        // do something after
        Debug.Log("After");
    
    }
}
