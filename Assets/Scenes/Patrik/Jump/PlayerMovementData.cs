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
    [SerializeField] private MidAirForces appliableAirForces;

    [Space, Header("Rotational")] 
    [SerializeField] private float rotationSpeed;
    
    #region Getters & Setters
    
    public float GetGravityMagnitudeUp => gravitationMagnitudeUp;
    public float GetGravityMagnitudeDown => gravitationMagnitudeDown;

    public float GetRayCastLength => rayCastLength;
    
    public float GetSneakSpeed => speedRelated.sneakSpeed;
    public float GetWalkSpeed => speedRelated.walkSpeed;
    public float GetRunSpeed => speedRelated.runSpeed;
    
    public JumpParameters GetNormalJump => normalJump;
    public JumpParameters GetMegaJump => megaJump;
    public MidAirForces GetMidAirForces => appliableAirForces;

    public float GetRotationSpeed => rotationSpeed;

    #endregion


}
