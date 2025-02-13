using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class KameraPrototyp : MonoBehaviour
{
    
    public float radius;
    public float height;
    

    public float angleH;
    public float angleP;
    public float pMax;
    public float pMin;

    public float gamepadSpeedMultiplier;
    public float mouseSpeedMultiplier;
    private float speedMultiplier;

    public float rotateSpeedH;
    public float rotateSpeedP;

    Camera cam;

    public Transform target;


    public Transform audioListener;
    private Vector2 camXandZ;

    private Vector2 delta;

    void Start()
    {
        //target = gameObject.transform;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation(speedMultiplier);
        RotateAudioListener();
        

        //audioListener.localRotation
    }

    public void LookAround(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            InputDevice device = context.control.device;
            delta = context.ReadValue<Vector2>();

            if(device is Gamepad)
            {
                speedMultiplier = gamepadSpeedMultiplier;
            }
            if (device is Mouse)
            {
                speedMultiplier = mouseSpeedMultiplier;
            }

        }

        if(context.canceled)
        {
            delta = Vector2.zero; 
        }
        
    }

    public void UpdateRotation(float multi)
    {
        angleH += delta.x * rotateSpeedH * multi * Time.deltaTime;

        angleP += delta.y * rotateSpeedP * multi * Time.deltaTime;
        angleP = Mathf.Clamp(angleP, pMax, pMin);


        Vector3 offset = new Vector3(0, height, -radius);
        Quaternion rotation = Quaternion.Euler(angleP, angleH, 0);
        cam.transform.position = target.position + rotation * offset;



        cam.transform.LookAt(target.position);
    }

    public void RotateAudioListener()
    {
        camXandZ.x = cam.transform.forward.x;
        camXandZ.y = cam.transform.forward.z;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(camXandZ.x, 0, camXandZ.y)).normalized;
        audioListener.rotation = targetRotation;
    }
}
