using System;
using UnityEngine;

[Serializable]
public struct CameraAngles
{
    [Tooltip("Max vertical angle of the camera")]
    public float pMax;

    [Tooltip("Min vertical angle of the camera")]
    public float pMin;
}

[Serializable]
public struct CameraRotationalSpeed
{
    [Tooltip("Horizontal rotation speed")]
    public float rotateSpeedH;

    [Tooltip("Vertical rotation speed")]
    public float rotateSpeedP;
}

[Serializable]
public struct DeviceSpeedMultiplier
{
    [Tooltip("Multiplier for rotation speed when using gamepad")]
    public float gamepadSpeedMultiplier;

    [Tooltip("Multiplier for ratation speed when using mouse")]
    public float mouseSpeedMultiplier;
}
