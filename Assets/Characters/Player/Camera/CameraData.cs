using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraDataSO")]
public class CameraData : ScriptableObject
{
    [Tooltip("Adjusts the distance between player and camera")]
    [SerializeField] private float radius;

    [Tooltip("Not working as intended. Adjust y-value of CameraFocusPoint child object for camera height ajustments")]
    [SerializeField] private float height;

    [SerializeField] private CameraAngles cameraAngleSettings;

    [SerializeField] private CameraRotationalSpeed cameraRotationalSpeedSettings;

    [SerializeField] private DeviceSpeedMultiplier deviceSettings;
    
    
    public float GetRadius
    {
        get { return radius; }
    }
    public float GetHeight
    {
        get { return height; }
    }


    public float GetPMax
    {
        get { return cameraAngleSettings.pMax; }
    }
    public float GetPMin
    {
        get { return cameraAngleSettings.pMin; }
    }


    public float GetRotateSpeedH
    {
        get { return cameraRotationalSpeedSettings.rotateSpeedH; }
    }
    public float GetRotateSpeedP
    {
        get { return cameraRotationalSpeedSettings.rotateSpeedP; }
    }


    public float GetGamepadSpeedMultiplier
    {
        get { return deviceSettings.gamepadSpeedMultiplier; }
    }
    public float GetMouseSpeedMultiplier
    {
        get { return deviceSettings.mouseSpeedMultiplier; }
    }
}
