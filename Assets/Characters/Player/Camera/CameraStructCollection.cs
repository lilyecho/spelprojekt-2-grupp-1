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

[Serializable]
public struct OffsetOptions
{
    [Tooltip("Adjusts the camera focus point above the player")]
    public Vector3 heightOffset;

    [Tooltip("Adjust how fast the camera snaps into place")]
    public float smoothTime;
}

