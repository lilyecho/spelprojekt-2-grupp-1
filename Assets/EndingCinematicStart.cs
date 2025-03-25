using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCinematicStart : MonoBehaviour
{
    
    public Animator animator;
    public GameObject image;
    
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        
        image.SetActive(true);
        animator.SetBool("EndingTrigger", true);
        
        StartCoroutine(Wait());
        
        
        
    }
    
    
    IEnumerator Wait()
    {
        // do something before
        Debug.Log("Before");

        // waits here
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("EndingTrigger", false);
        // gets the curent screen
        Scene sceneLoaded = SceneManager.GetActiveScene();
        // loads next level
        SceneManager.LoadScene(sceneLoaded.buildIndex + 1);

        // do something after
        Debug.Log("After");
    
    }
    
    
}
