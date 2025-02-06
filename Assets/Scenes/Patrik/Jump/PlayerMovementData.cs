using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Player/PlayerMovementData")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Player Physics")] 
    [SerializeField, Min(0)] private float gravitationMagnitude; 
    
    [Header("Jump-Related")]
    [SerializeField] private JumpParameters normalJump;
    [SerializeField] private JumpParameters megaJump;
    [SerializeField] private MidAirForces appliedAirForces;

    public JumpParameters GetNormalJump => normalJump;
    public JumpParameters GetMegaJump => megaJump;

    public float GetGravityMagnitude => gravitationMagnitude;
}
