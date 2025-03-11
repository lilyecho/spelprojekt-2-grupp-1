using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Characters/Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    //TODO validation
    [Header("Player Physics")] 
    [SerializeField, Min(0)] private float gravitationMagnitudeUp; 
    [SerializeField, Min(0)] private float gravitationMagnitudeDown;
    [SerializeField] private float slopeCheckerLength;
    [SerializeField] private float maxRotationAngle;

    [Header("Surface-normal Checker")] 
    [SerializeField, Min(0)] 
    private float rayCastLength;

    [Space] 
    [SerializeField] private SpeedRelated speedRelated;

    [SerializeField] private SoundAlertingRanges alertingRanges;
    
    [Space,Header("Jump-Related")]
    [SerializeField] private JumpParameters normalJump;
    [SerializeField] private JumpParameters megaJump;
    [SerializeField, Tooltip("Data according to X-Z plane")] private MidAirForces appliableAirForces;
    [SerializeField] private float chargeTime;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBufferDuration;
    [SerializeField] private float glideMinimumHeight;
    [SerializeField] private float glideExitHeight;
    [SerializeField] private float glideFallingSpeed;

    [SerializeField] private ShrinkRelated shrinkRelated;
    
    [Space, Header("Rotational")] 
    [SerializeField] private float rotationSpeed;

    [Header("Extra"), Tooltip("Mostly for the use of shrinking")] 
    [SerializeField, Range(0.01f, 1)] private float factorialValue4Shrink = 1;
    
    
    
    #region Getters & Setters
    
    public float GetGravityMagnitudeUp => gravitationMagnitudeUp * factorialValue4Shrink;
    public float GetGravityMagnitudeDown => gravitationMagnitudeDown * factorialValue4Shrink;
    public float GetRayCastLength => rayCastLength * factorialValue4Shrink;
    public SpeedRelated GetSpeedRelated => speedRelated;
    public SoundAlertingRanges GetAlertingRanges => alertingRanges;
    public JumpParameters GetNormalJump => normalJump;
    public JumpParameters GetMegaJump => megaJump;
    public MidAirForces GetMidAirForces => appliableAirForces;
    public float GetChargeTime => chargeTime;
    public float GetCoyoteTime => coyoteTime;
    public float GetJumpBufferDuration => jumpBufferDuration;
    public float GetGlideMinimumHeight => glideMinimumHeight;
    public float GetGlideExitHeight => glideExitHeight;
    public float GetGlideFallingSpeed => glideFallingSpeed;
    public ShrinkRelated GetShrinkParameters => shrinkRelated;
    public float GetRotationSpeed => rotationSpeed;
    public float GetSlopeCheckerLength => slopeCheckerLength;
    public float GetMaxRotationAngle => maxRotationAngle;
    public Vector3 CharacterScale => new Vector3(1, 1, 1) * factorialValue4Shrink;

    #endregion
    
}
