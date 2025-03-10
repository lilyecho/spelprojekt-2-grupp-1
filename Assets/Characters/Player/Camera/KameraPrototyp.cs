using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class KameraPrototyp : MonoBehaviour
{
    [SerializeField] public CameraData cameraData;
    [SerializeField] public CameraData normalCameraData;
    [SerializeField] public CameraData shrinkedCameraData;
    public CameraData GetCameraData
    {
        get { return cameraData;}
    }
    /*
    private float radius;
    private float height;


    
    private float pMax;
    private float pMin;

    private float gamepadSpeedMultiplier;
    private float mouseSpeedMultiplier;
    

    private float rotateSpeedH;
    private float rotateSpeedP;
    */
    Camera cam;

    public Transform target;

    private GameObject targetPoint;
    private Transform target2;
    public Vector3 heightOffset;
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;

    private float speedMultiplier;
    public float angleH;
    public float angleP;

    public Transform audioListener;
    private Vector2 camXandZ;

    private Vector2 delta;

    void Start()
    {
        //target = gameObject.transform;
        cam = Camera.main;

        /*
        radius = GetCameraData.GetRadius;
        height = GetCameraData.GetHeight;
        pMax = GetCameraData.GetPMax;
        pMin = GetCameraData.GetPMin;
        gamepadSpeedMultiplier = GetCameraData.GetGamepadSpeedMultiplier;
        mouseSpeedMultiplier = GetCameraData.GetMouseSpeedMultiplier;
        rotateSpeedH = GetCameraData.GetRotateSpeedH;
        rotateSpeedP = GetCameraData.GetRotateSpeedP;
        */

        targetPoint = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation(speedMultiplier);
        RotateAudioListener();


        //audioListener.localRotation
        targetPoint.transform.position = Vector3.SmoothDamp(targetPoint.transform.position, transform.position + GetCameraData.GetHeightOffset, ref velocity, GetCameraData.GetSmoothTime);
    }

    public void LookAround(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            InputDevice device = context.control.device;
            delta = context.ReadValue<Vector2>();

            if(device is Gamepad)
            {
                speedMultiplier = GetCameraData.GetGamepadSpeedMultiplier;
            }
            if (device is Mouse)
            {
                speedMultiplier = GetCameraData.GetMouseSpeedMultiplier;
            }

        }

        if(context.canceled)
        {
            delta = Vector2.zero; 
        }
        
    }

    public void UpdateRotation(float multi)
    {
        angleH += delta.x * GetCameraData.GetRotateSpeedH * multi * Time.deltaTime;

        angleP += delta.y * GetCameraData.GetRotateSpeedP * multi * Time.deltaTime;
        angleP = Mathf.Clamp(angleP, GetCameraData.GetPMax, GetCameraData.GetPMin);


        Vector3 offset = new Vector3(0, GetCameraData.GetHeight, -GetCameraData.GetRadius);
        Quaternion rotation = Quaternion.Euler(angleP, angleH, 0);
        //cam.transform.position = target.position + rotation * offset;
        cam.transform.position = targetPoint.transform.position + rotation * offset;



        cam.transform.LookAt(targetPoint.transform.position);
    }

    public void RotateAudioListener()
    {
        camXandZ.x = cam.transform.forward.x;
        camXandZ.y = cam.transform.forward.z;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(camXandZ.x, 0, camXandZ.y)).normalized;
        audioListener.rotation = targetRotation;
    }
}
