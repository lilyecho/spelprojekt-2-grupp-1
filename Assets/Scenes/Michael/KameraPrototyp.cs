using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KameraPrototyp : MonoBehaviour
{

    public float radius = 5;
    public float angleH;
    public float angleP;
    public float rotateSpeedH;
    public float rotateSpeedP;

    Camera cam = Camera.main;

    private Vector2 delta;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        angleH += rotateSpeedH * Time.deltaTime;
        angleP += rotateSpeedP * Time.deltaTime;





        cam.transform.LookAt(transform.position);
    }

    public void LookAround(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            delta = context.ReadValue<Vector2>();
        }

        if(context.canceled)
        {
            delta = Vector2.zero; 
        }
        
    }
}
