using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Player/PlayerMovementData")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Player Physics")] 
    [SerializeField, Min(0)] private float gravitationMagnitudeUp; 
    [SerializeField, Min(0)] private float gravitationMagnitudeDown; 
    
    [Header("Jump-Related")]
    [SerializeField] private JumpParameters normalJump;
    [SerializeField] private JumpParameters megaJump;
    [SerializeField] private MidAirForces appliableAirForces;

    public JumpParameters GetNormalJump => normalJump;
    public JumpParameters GetMegaJump => megaJump;
    public MidAirForces GetMidAirForces => appliableAirForces;

    public float GetGravityMagnitudeUp => gravitationMagnitudeUp;
    public float GetGravityMagnitudeDown => gravitationMagnitudeDown;
    
}
