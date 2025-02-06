using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Player/PlayerMovementData")]
public class PlayerMovementData : ScriptableObject
{
    [SerializeField] private JumpParameters normalJump;
    [SerializeField] private JumpParameters megaJump;
}
