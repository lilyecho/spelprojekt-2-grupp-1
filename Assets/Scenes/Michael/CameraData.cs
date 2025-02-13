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

    [Tooltip("Max vertical angle of the camera")]
    [SerializeField] private float pMax;

    [Tooltip("Min vertical angle of the camera")]
    [SerializeField] private float pMin;

    [Tooltip("Horizontal rotation speed")]
    [SerializeField] private float rotateSpeedH;

    [Tooltip("Vertical rotation speed")]
    [SerializeField] private float rotateSpeedP;

    [Tooltip("Multiplier for rotation speed when using gamepad")]
    [SerializeField] private float gamepadSpeedMultiplier;

    [Tooltip("Multiplier for ratation speed when using mouse")]
    [SerializeField] private float mouseSpeedMultiplier;
    void Start()
    {
        
    }

    

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
        get { return pMax; }
    }
    public float GetPMin
    {
        get { return pMin; }
    }


    public float GetRotateSpeedH
    {
        get { return rotateSpeedH; }
    }
    public float GetRotateSpeedP
    {
        get { return rotateSpeedP; }
    }


    public float GetGamepadSpeedMultiplier
    {
        get { return gamepadSpeedMultiplier; }
    }
    public float GetMouseSpeedMultiplier
    {
        get { return mouseSpeedMultiplier; }
    }
}
