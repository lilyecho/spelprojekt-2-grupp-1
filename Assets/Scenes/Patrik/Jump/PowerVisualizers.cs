using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PowerVisualizers : MonoBehaviour
{
    [SerializeField] private PlayerMovementData playerMovementData = null;
    [SerializeField] private PlayerQualities visualizePlayerQualities;

    [SerializeField, Min(4)] private int resolutionAreaMarkers=4;
    [SerializeField, Min(0)] private float maxLengthPointPlacement=4; 
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
        if (visualizePlayerQualities.HasFlag(PlayerQualities.Jump))
        {
            Gizmos.color = Color.blue;
            DrawJumpHeight(playerMovementData.GetNormalJump.GetJumpHeight);
            DrawAreaOfLandingFromPoint(transform.position + new Vector3(0,playerMovementData.GetNormalJump.GetJumpHeight,0),resolutionAreaMarkers);
        }
        
        if (visualizePlayerQualities.HasFlag(PlayerQualities.MegaJump))
        {
            Gizmos.color = Color.red;
            DrawJumpHeight(playerMovementData.GetMegaJump.GetJumpHeight);
            DrawAreaOfLandingFromPoint(transform.position + new Vector3(0,playerMovementData.GetMegaJump.GetJumpHeight,0),resolutionAreaMarkers);
        }
        
        
    }

    private void DrawJumpHeight(float jumpHeight)
    {
        Vector3 cubePosition = transform.position + new Vector3(0,jumpHeight,0);
        Gizmos.DrawCube(cubePosition, new Vector3(.1f,.1f,.1f));
    }

    private void DrawAreaOfLandingFromPoint(Vector3 startPosition,int resolution)
    {
        Vector3 startDirection = new Vector3(0,0,1);
        float ratio = playerMovementData.GetGravityMagnitudeDown /
                      playerMovementData.GetMidAirForces.GetAppliedMagnitude;
        
        float angleOfFall = Mathf.Atan(ratio)*Mathf.Rad2Deg;
        
        float angleBetweenPoints = 360f / resolution;
        
        
        for (int i = 0; i < resolution; i++)
        {
            float angleCurrent = angleBetweenPoints * i;
            
            Quaternion rotationY = Quaternion.AngleAxis(angleCurrent, Vector3.up);
            Quaternion rotationX = Quaternion.AngleAxis(angleOfFall, Vector3.right);
            
            Quaternion finalRotation = rotationY * rotationX;

            Vector3 finalDirection = finalRotation * startDirection;

            Physics.Raycast(startPosition, finalDirection, out RaycastHit hit, 10f);
            
            Gizmos.DrawSphere(hit.point,.1f);


            /*
            //2D vector for rotating around axis
            Vector2 directionTwoD = RotateVectorClock(startDirection, angleCurrent);
            //3D for the angle of fall
            float
            Vector3 aimDirectionThreeD = new Vector3(directionTwoD.x,,directionTwoD.y);
            Physics.Raycast(startPosition,aimDirectionThreeD)*/
        }
    }
}
