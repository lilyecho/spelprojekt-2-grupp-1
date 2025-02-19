using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Characters/Player/AbilityData")]
public class AbilityData : ScriptableObject
{
     [Flags]
     public enum Abilities
     {
          MegaJump = 1,
          Glide = 2
     }

     [SerializeField] private Abilities activeAbilities;

     public void ActivateAbilities(Abilities newAbility)
     {
          activeAbilities |= newAbility;
     }

     public void ResetAbilities()
     {
          activeAbilities = 0;
     }
}
