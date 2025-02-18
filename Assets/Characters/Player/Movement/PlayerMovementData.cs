using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Player/PlayerMovementData")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Player Physics")] 
    [SerializeField, Min(0)] private float gravitationMagnitudeUp; 
    [SerializeField, Min(0)] private float gravitationMagnitudeDown;

    [Header("Surface-normal Checker")] 
    [SerializeField, Min(0)] 
    private float rayCastLength;

    [Space] 
    [SerializeField] private SpeedRelated speedRelated;
    
    [Space,Header("Jump-Related")]
    [SerializeField] private JumpParameters normalJump;
    [SerializeField] private JumpParameters megaJump;
    [SerializeField, Tooltip("Data according to X-Z plane")] private MidAirForces appliableAirForces;
    [SerializeField] private float chargeTime;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBufferDuration;

    [Space, Header("Rotational")] 
    [SerializeField] private float rotationSpeed;
    
    #region Getters & Setters
    
    public float GetGravityMagnitudeUp => gravitationMagnitudeUp;
    public float GetGravityMagnitudeDown => gravitationMagnitudeDown;

    public float GetRayCastLength => rayCastLength;

    public SpeedRelated GetSpeedRelated => speedRelated;
    
    public JumpParameters GetNormalJump => normalJump;
    public JumpParameters GetMegaJump => megaJump;
    public MidAirForces GetMidAirForces => appliableAirForces;
    public float GetChargeTime => chargeTime;
    public float GetCoyoteTime => coyoteTime;
    public float GetJumpBufferDuration => jumpBufferDuration;

    public float GetRotationSpeed => rotationSpeed;

    #endregion


}
