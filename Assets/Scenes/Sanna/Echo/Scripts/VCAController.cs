using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class VCAController : MonoBehaviour
{
    
    // VCA variables
    private VCA vca; 
    public string vcaName;
    private float volume = 1;

    private Slider masterSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        vca = RuntimeManager.GetVCA("vca:/" + vcaName); // apply the VCA to the vca variable
        masterSlider = GetComponent<Slider>(); // apply the slider component to the masterslider variable
    }

    public void setVolume(float volume)
    {
        vca.setVolume(volume); // set the volume to the vca
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}