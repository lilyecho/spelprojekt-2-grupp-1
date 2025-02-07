using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PowerUpVisualizers : MonoBehaviour
{
    [SerializeField] private PlayerMovementData playerMovementData = null;
    [SerializeField] private PlayerQualities visualizePlayerQualities;
    
    [Flags]
    private enum PlayerQualities
    {
        Jump = 1,
        MegaJump = 2,
        Glide = 4
    }

    private void OnDrawGizmos()
    {
        if (playerMovementData == null) return;

        //Jump & MegaJump
        if (visualizePlayerQualities.CompareTo(PlayerQualities.Jump) != 0)
        {
            DrawJump(playerMovementData.GetNormalJump.GetJumpHeight);
        }
        
        if (visualizePlayerQualities.CompareTo(PlayerQualities.MegaJump) != 0)
        {
            DrawJump(playerMovementData.GetMegaJump.GetJumpHeight);
        }
        
        
    }

    private void DrawJump(float jumpHeight)
    {
        Gizmos.color = Color.red;
        Vector3 cubePosition = transform.position + new Vector3(0,jumpHeight,0);
        Gizmos.DrawCube(cubePosition, new Vector3(.1f,.1f,.1f));
    }
}
