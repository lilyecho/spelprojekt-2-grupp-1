using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    public EventInfo[] soundInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.InvokeEventInfo(soundInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
