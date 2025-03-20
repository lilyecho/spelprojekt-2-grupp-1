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
    
    public enum State { MOVING, CAUGHT};
    public State state = State.MOVING;
    public CameraTrollPort cameraTrollPort;

    Camera cam;

    public Transform target;

    private GameObject targetPoint;
    
    public Vector3 heightOffset;
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;

    private float speedMultiplier;
    public float angleH;
    public float angleP;

    public Transform audioListener;
    private Vector2 camXandZ;

    private Vector2 delta;



    private float extraP = 0;
    private float extraHeightOffset = 1;
    private float extraRadius = 0f;


    private Transform trollPos;
    public Transform camPosWhenCaught;


    void Start()
    {
        cam = Camera.main;
        
        targetPoint = new GameObject();
        targetPoint.transform.position = transform.position + heightOffset;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
            {
                case State.MOVING:
                    UpdateRotation(speedMultiplier);
                    targetPoint.transform.position = Vector3.SmoothDamp(targetPoint.transform.position, 
                                                     transform.position + GetCameraData.GetHeightOffset * extraHeightOffset, ref velocity, GetCameraData.GetSmoothTime);
                break;

                case State.CAUGHT:
                    targetPoint.transform.position = Vector3.SmoothDamp(targetPoint.transform.position, trollPos.transform.position, ref velocity, 0.05f);
                cam.transform.position = camPosWhenCaught.position;//Vector3.SmoothDamp(cam.transform.position, camPosWhenCaught.position, ref velocity, 0.02f);
                    cam.transform.LookAt(targetPoint.transform.position /* + heightOffset*/);
                break;
            }
        //UpdateRotation(speedMultiplier);
        //targetPoint.transform.position = Vector3.SmoothDamp(targetPoint.transform.position, transform.position + GetCameraData.GetHeightOffset, ref velocity, GetCameraData.GetSmoothTime);
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
        if(extraP == 0 )
        {
            angleH += delta.x * GetCameraData.GetRotateSpeedH * multi * GameManager.instance.cameraSensitivity * Time.deltaTime;

            angleP += delta.y * GetCameraData.GetRotateSpeedP * multi * GameManager.instance.cameraSensitivity * Time.deltaTime;
            angleP = Mathf.Clamp(angleP, GetCameraData.GetPMax, GetCameraData.GetPMin);
        }

        
        
        /*
        if (angleP == GetCameraData.GetPMin && delta.y > 0)
        {
            extraP += delta.y * GetCameraData.GetRotateSpeedP * multi * GameManager.instance.cameraSensitivity * Time.deltaTime;
            angleH += delta.x * GetCameraData.GetRotateSpeedH * multi * GameManager.instance.cameraSensitivity * Time.deltaTime;
            extraP = Mathf.Clamp(extraP, 0, 35);
            extraHeightOffset = 1 + 1.5f * (extraP / 35);
            extraRadius = .5f * (extraP / 35);

        }

        if(extraP > 0 && delta.y < 0)
        {
            extraP += delta.y * GetCameraData.GetRotateSpeedP * multi * GameManager.instance.cameraSensitivity * Time.deltaTime;
            angleH += delta.x * GetCameraData.GetRotateSpeedH * multi * GameManager.instance.cameraSensitivity * Time.deltaTime;
            extraP = Mathf.Clamp(extraP, 0, 35);
            extraHeightOffset = 1 + 1.5f * (extraP / 35);
            extraRadius = .5f * (extraP / 35);
        }
        */

        Vector3 offset = new Vector3(0, GetCameraData.GetHeight, -GetCameraData.GetRadius);

        

        Quaternion rotation = Quaternion.Euler(angleP + extraP, angleH, 0);
        //cam.transform.position = target.position + rotation * offset;
        cam.transform.position = targetPoint.transform.position + rotation * offset;
        
        cam.transform.LookAt(targetPoint.transform.position);
    }


    


    public void OnAstridCaught(CameraTrollPort cameraTrollPort, Transform troll, Transform cameraPos)
    {

        /*
        targetPoint.transform.position = Vector3.SmoothDamp(targetPoint.transform.position, troll.position + GetCameraData.GetHeightOffset, ref velocity, GetCameraData.GetSmoothTime);
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, cameraPos.position, ref velocity, GetCameraData.GetSmoothTime);
        cam.transform.LookAt(targetPoint.transform.position);
        */
        state = State.CAUGHT;
        trollPos = troll;
        camPosWhenCaught = cameraPos;
        angleH = 0;
        angleP = 0;
        
    }

    public void CameraAfterRespawn(CameraTrollPort cameraTrollPort)
    {
        state = State.MOVING;
        targetPoint.transform.position = transform.position + heightOffset;
    }



    private void OnEnable()
    {
        cameraTrollPort.OnAstridGettingCaught += OnAstridCaught;
        cameraTrollPort.CameraAfterRespawn += CameraAfterRespawn;
    }

    private void OnDisable()
    {
        cameraTrollPort.OnAstridGettingCaught -= OnAstridCaught;
        cameraTrollPort.CameraAfterRespawn -= CameraAfterRespawn;
    }

}
