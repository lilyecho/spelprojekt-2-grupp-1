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
    


    public float rotateSpeedH;
    public float rotateSpeedP;

    Camera cam;

    public Transform target;

    private Vector2 delta;

    void Start()
    {
        //target = gameObject.transform;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
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

    public void UpdateRotation()
    {
        angleH += delta.x * rotateSpeedH * Time.deltaTime;

        angleP += delta.y * rotateSpeedP * Time.deltaTime;
        angleP = Mathf.Clamp(angleP, pMax, pMin);


        Vector3 offset = new Vector3(0, height, -radius);
        Quaternion rotation = Quaternion.Euler(angleP, angleH, 0);
        cam.transform.position = target.position + rotation * offset;



        cam.transform.LookAt(target.position);
    }
}
