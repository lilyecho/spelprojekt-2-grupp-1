using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PowerUpVisualizers : MonoBehaviour
{
    [SerializeField] private PlayerMovementData playerMovementData = null;
    [SerializeField] private PlayerQualities visualizePlayerQualities;

    [SerializeField, Min(4)] private int resolutionAreaMarkers;
    
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
            DrawJumpHeight(playerMovementData.GetNormalJump.GetJumpHeight);
        }
        
        if (visualizePlayerQualities.CompareTo(PlayerQualities.MegaJump) != 0)
        {
            DrawJumpHeight(playerMovementData.GetMegaJump.GetJumpHeight);
        }
        
        //DrawAreaOfLandingFromPoint(transform.position,4);
    }

    private void DrawJumpHeight(float jumpHeight)
    {
        Gizmos.color = Color.red;
        Vector3 cubePosition = transform.position + new Vector3(0,jumpHeight,0);
        Gizmos.DrawCube(cubePosition, new Vector3(.1f,.1f,.1f));
    }

    private void DrawAreaOfLandingFromPoint(Vector3 startPosition,int resolution)
    {
        Vector3 startDirection = new Vector3(0,0,1);
        float ratio = playerMovementData.GetGravityMagnitudeDown /
                      playerMovementData.GetMidAirForces.GetAppliedMagnitude;
        
        //float angleOfFall = Mathf.Sin(1);
        
        float angleBetweenPoints = 360f / resolution;
        
        for (int i = 0; i < resolution; i++)
        {
            float angleCurrent = angleBetweenPoints * i;
            
            Quaternion rotationY = Quaternion.AngleAxis(angleCurrent, Vector3.up);
            //Quaternion rotationX = Quaternion.AngleAxis(angleOfFall, Vector3.right);
            
            //Quaternion finalRotation = rotationX * rotationY;

            //Vector3 finalDirection = finalRotation * startDirection;
            
            //Debug.Log(finalDirection.ToString());
            
            /*
            //2D vector for rotating around axis
            Vector2 directionTwoD = RotateVectorClock(startDirection, angleCurrent);
            //3D for the angle of fall
            float 
            Vector3 aimDirectionThreeD = new Vector3(directionTwoD.x,,directionTwoD.y);
            Physics.Raycast(startPosition,aimDirectionThreeD)*/
        }
    }
    
    private Vector2 RotateVectorCounter(Vector2 inputVector, float angle)
    {
        if (angle <= 0) throw new ArgumentException("RotateVectorCounter can't and shouldn't handle angle less or equal to 0");
        
        float vectorX = inputVector.x * Mathf.Cos(Mathf.Deg2Rad * angle) +
                        inputVector.y * -Mathf.Sin(Mathf.Deg2Rad * angle);
        float vectorY = inputVector.x * Mathf.Sin(Mathf.Deg2Rad * angle) +
                        inputVector.y * Mathf.Cos(Mathf.Deg2Rad * angle);

        return new Vector2(vectorX, vectorY);
    }
    private Vector2 RotateVectorClock(Vector2 inputVector, float angle)
    {
        if (angle <= 0) throw new ArgumentException("RotateVectorCounter can't and shouldn't handle angle less or equal to 0");
        
        float vectorX = inputVector.x * Mathf.Cos(Mathf.Deg2Rad * angle) +
                        inputVector.y * Mathf.Sin(Mathf.Deg2Rad * angle);
        float vectorY = inputVector.x * -Mathf.Sin(Mathf.Deg2Rad * angle) +
                        inputVector.y * Mathf.Cos(Mathf.Deg2Rad * angle);

        return new Vector2(vectorX, vectorY);
    }
    
    
}
