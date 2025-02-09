using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PowerVisualizers : MonoBehaviour
{
    [SerializeField] private PlayerMovementData playerMovementData = null;
    [SerializeField] private PlayerQualities visualizePlayerQualities;

    [SerializeField, Min(4)] private int resolutionAreaMarkers=4;
    [SerializeField, Min(0)] private float maxLengthPointPlacement=4;

    [SerializeField]private Rigidbody rigidB = null;
    
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
            FindFurthestPointWhenJumping(transform.position,playerMovementData.GetNormalJump.GetJumpHeight,resolutionAreaMarkers);
        }
        
        if (visualizePlayerQualities.HasFlag(PlayerQualities.MegaJump))
        {
            Gizmos.color = Color.red;
            DrawJumpHeight(playerMovementData.GetMegaJump.GetJumpHeight);
            FindFurthestPointWhenJumping(transform.position,playerMovementData.GetMegaJump.GetJumpHeight,resolutionAreaMarkers);
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

            Physics.Raycast(startPosition, finalDirection, out RaycastHit hit, maxLengthPointPlacement);
            Gizmos.DrawSphere(hit.point,.1f);
        }
    }

    private void FindFurthestPointWhenJumping(Vector3 playerPosition,float jumpHeight,int resolution)
    {
        //Setting up. Front is first point on ascending-path
        Vector3 startDirection = Vector3.forward;
        float startForce = PhysicsCalculations.ForceToJumpCertainHeight(jumpHeight, rigidB.mass,playerMovementData.GetGravityMagnitudeUp);
        Vector3 maxWidthVelocity = (startDirection * playerMovementData.GetMidAirForces.GetAppliedMagnitude +
                                        Vector3.up * startForce);

        float startSpeed = maxWidthVelocity.magnitude;
        float angleOfIncrease = Vector3.Angle(startDirection, maxWidthVelocity.normalized);
        float realTime = PhysicsCalculations.ProjectileCertainTimeOfMaxHeight(startSpeed, angleOfIncrease, playerMovementData.GetGravityMagnitudeUp);
        
        float angleBetweenPoints = 360f / resolution;

        Vector3[] heightPoints = GetFarMaxHeightPoints(playerPosition,jumpHeight,resolution,angleBetweenPoints,maxWidthVelocity,angleOfIncrease);

        for (int i = 0; i < heightPoints.Length; i++)
        {
            float angleCurrent = angleBetweenPoints * i;
            if (!IsAtExtremeHeightPoints(playerPosition,jumpHeight,heightPoints[i], startSpeed, angleOfIncrease, realTime, out RaycastHit hit))
            {
                Gizmos.DrawSphere(hit.point, 0.1f);
            }
            else
            {
                FindCorrespondingCollisionPointDescending(heightPoints[i], angleCurrent);
            }
        }
    }
    
    private Vector3[] GetFarMaxHeightPoints(Vector3 playerPosition, float jumpHeight,int resolution, float angleBetweenPoints, Vector3 maxWidthVelocity, float angleOfIncrease)
    {
        List<Vector3> result = new List<Vector3>();

        for (int i = 0; i < resolution; i++)
        {
            float angleCurrent = angleBetweenPoints * i;
            
            Quaternion rotationY = Quaternion.AngleAxis(angleCurrent, Vector3.up);
            
            Vector3 finalPoint = rotationY * maxWidthVelocity;
            
            float startSpeed = finalPoint.magnitude;
        
            float realTime = PhysicsCalculations.ProjectileCertainTimeOfMaxHeight(startSpeed, angleOfIncrease, playerMovementData.GetGravityMagnitudeUp);
            
            float maxDistanceValue =
                PhysicsCalculations.ProjectileCertainWidthFromRealTime(startSpeed, angleOfIncrease, realTime);

            Vector3 curvePoint = playerPosition + rotationY*new Vector3(0,jumpHeight,maxDistanceValue);
            result.Add(curvePoint);
        }
        
        return result.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>True if it didnt collide with anything</returns>
    private bool IsAtExtremeHeightPoints(Vector3 playerPosition, float jumpHeight, Vector3 heightPoint, float startSpeed, float angleOfIncrease, float realTime, out RaycastHit raycastHit)
    {
        Vector3 directionToPointRayCast = (heightPoint-playerPosition).normalized;
            
        float maxDistanceValue = PhysicsCalculations.ProjectileCertainWidthFromRealTime(startSpeed, angleOfIncrease, realTime);
        float maxDistanceRayCast = MathF.Sqrt(maxDistanceValue * maxDistanceValue + jumpHeight * jumpHeight);
            
        if (Physics.Raycast(playerPosition, directionToPointRayCast, out RaycastHit hit, maxDistanceRayCast))
        {
            raycastHit = hit;
            return false;
        }
        
        raycastHit = hit;
        return true;
    }

    private void FindCorrespondingCollisionPointDescending(Vector3 guidHeightPoint, float angleCurrent)
    {
        //Point on descending   
                
        float ratio = playerMovementData.GetGravityMagnitudeDown /
                      playerMovementData.GetMidAirForces.GetAppliedMagnitude;
        
        float angleOfFall = Mathf.Atan(ratio)*Mathf.Rad2Deg;
        
        Quaternion rotationY = Quaternion.AngleAxis(angleCurrent, Vector3.up);
        Quaternion rotationX = Quaternion.AngleAxis(angleOfFall, Vector3.right);
            
        Quaternion finalRotation = rotationY * rotationX;
        Vector3 directionToCollision = finalRotation * Vector3.forward;
        
        
        Physics.Raycast(guidHeightPoint, directionToCollision, out RaycastHit hit, maxLengthPointPlacement);
        Gizmos.DrawSphere(hit.point,.1f);
    }
}
