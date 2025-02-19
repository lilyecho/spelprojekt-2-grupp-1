using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class State
{


    public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }
    public virtual void OnCollision(Collision collision)
    {

    }

    public virtual void Update()
    {

    }
    public virtual void FixedUpdate()
    {

    }

    public virtual void OnSpaceBar(InputAction.CallbackContext context)
    {

    }
    public virtual void OnShift(InputAction.CallbackContext context)
    {

    }
    public virtual void OnCTRL(InputAction.CallbackContext context)
    {

    }

    public virtual void OnWASD(InputAction.CallbackContext context)
    {

    }
    public virtual void OnMOUSE(InputAction.CallbackContext context)
    {

    }


    protected PlayerBehaviour playerBehaviour;


    public State(PlayerBehaviour playerBehaviour)
    {
        this.playerBehaviour = playerBehaviour;
    }

    protected bool CheckForGround(Transform[] raycastPoints, float rayCastLength)
    {
        LayerMask layerToIgnore = (1 << 8) | (1 << 2);
        RaycastHit hit;
        foreach (Transform t in raycastPoints)
        {
            if (Physics.Raycast(t.position, Vector3.down, out hit, rayCastLength, ~layerToIgnore))
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                
                if (angle < 30f)
                {
                    return true;
                }
                
            }
        }
        return false;
    }


    protected Quaternion AlignToSlope(Transform[] rayCastPoints, Transform playerTransform, Vector3 normal, float slopeCheckerLength, float maxRotationAngle)
    {
        //
        LayerMask layerToIgnore = (1 << 8) | (1 << 2);
        Vector3 point1 = Vector3.zero;
        Vector3 point2 = Vector3.zero;
        RaycastHit hit1;
        RaycastHit hit2;
        if (Physics.Raycast(rayCastPoints[1].position, Vector3.down, out hit1, slopeCheckerLength, ~layerToIgnore))
        {
            if(Vector3.Angle(Vector3.up, hit1.normal) < maxRotationAngle)
            {
                point1 = hit1.point;
            }
            
        }
        if (Physics.Raycast(rayCastPoints[2].position, Vector3.down, out hit2, slopeCheckerLength, ~layerToIgnore))
        {
            if (Vector3.Angle(Vector3.up, hit2.normal) < maxRotationAngle)
            {
                point2 = hit2.point;
            }
            
        }

        //bool point1hit = Physics.Raycast(playerBehaviour.rayCastPoints[1].position, Vector3.down, out hit1, 2f, ~layerToIgnore);
        //bool point2hit = Physics.Raycast(playerBehaviour.rayCastPoints[2].position, Vector3.down, out hit2, 2f, ~layerToIgnore);

        Quaternion targetRotation;
        Vector3 vectorBetweenPoints;
        if (point1 != Vector3.zero && point2 != Vector3.zero)
        {
            vectorBetweenPoints = (point1 - point2).normalized;
            targetRotation = Quaternion.FromToRotation(playerTransform.forward, vectorBetweenPoints) * playerTransform.rotation;
        }
        else
        {
            //targetRotation = Quaternion.FromToRotation(playerTransform.up, normal) * playerTransform.rotation;
            targetRotation = Quaternion.FromToRotation(playerTransform.up, Vector3.up) * playerTransform.rotation;
        }

        
        //playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, targetRotation, timeCount);

        //timeCount = timeCount + 2 * Time.deltaTime;
        return targetRotation;
    }

    protected void AlignToSlope2(Transform[] rayCastPoints, Transform playerTransform, float slopeCheckerLength, float maxRotationAngle, ref float currentVelocity, float smoothTime)
    {
        //
        LayerMask layerToIgnore = (1 << 8) | (1 << 2);
        Vector3 point1 = Vector3.zero;
        Vector3 point2 = Vector3.zero;
        RaycastHit hit1;
        RaycastHit hit2;
        if (Physics.Raycast(rayCastPoints[1].position, Vector3.down, out hit1, slopeCheckerLength, ~layerToIgnore))
        {
            if (Vector3.Angle(Vector3.up, hit1.normal) < maxRotationAngle)
            {
                point1 = hit1.point;
            }

        }
        if (Physics.Raycast(rayCastPoints[2].position, Vector3.down, out hit2, slopeCheckerLength, ~layerToIgnore))
        {
            if (Vector3.Angle(Vector3.up, hit2.normal) < maxRotationAngle)
            {
                point2 = hit2.point;
            }

        }

        
        Vector3 vectorBetweenPoints;
        if (point1 != Vector3.zero && point2 != Vector3.zero)
        {
            vectorBetweenPoints = (point1 - point2).normalized;
            
        }
        else
        {
            
            vectorBetweenPoints = new Vector3(playerTransform.forward.x, 1, playerTransform.forward.z);
        }
        //float targetAngle = Mathf.Atan2(vectorBetweenPoints.y, vectorBetweenPoints.z) * Mathf.Rad2Deg;
        float targetAngle = Vector3.Angle(playerTransform.forward, vectorBetweenPoints);

        float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.x, targetAngle, ref currentVelocity, smoothTime);


        playerTransform.rotation = Quaternion.Euler(angle, playerTransform.eulerAngles.y, playerTransform.eulerAngles.z);
    }

    public Vector3 GetSurfaceNormal(Transform[] raycastPoints, float rayCastLength)
    {
        LayerMask layerToIgnore = (1 << 8) | (1 << 2);
        
        RaycastHit hit;
        if (Physics.Raycast(raycastPoints[1].position, Vector3.down, out hit, rayCastLength, ~layerToIgnore))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);

            if (angle < 30f)
            {
                return hit.normal;
            }
            
        }
        else if (Physics.Raycast(raycastPoints[0].position, Vector3.down, out hit, rayCastLength, ~layerToIgnore))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);

            if (angle < 30f)
            {
                return hit.normal;
            }
        }
        else if (Physics.Raycast(raycastPoints[2].position, Vector3.down, out hit, rayCastLength, ~layerToIgnore))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);

            if (angle < 30f)
            {
                return hit.normal;
            }
        }

        //return Vector3.up;
        return Vector3.zero;
    }

    private float CalculateNextSpeed(float maxSpeed, float time, float totalAccelerationTime)
    {
        if (playerBehaviour.rb.velocity.magnitude >= maxSpeed)
        {
            Debug.Log("MaxSpeed");
            return maxSpeed;
        }
        
        float tValue = Mathf.Clamp(time / totalAccelerationTime,0,1);
        return Mathf.Lerp(0, maxSpeed, tValue);

    }

    protected void ApplyAcceleration(float maxSpeed, float accelerationTotalTime)
    {
        playerBehaviour.accTime += Time.fixedDeltaTime;
        playerBehaviour.moveSpeed = CalculateNextSpeed(maxSpeed,playerBehaviour.accTime, accelerationTotalTime);
        playerBehaviour.rb.velocity = playerBehaviour.moveDir.normalized * playerBehaviour.moveSpeed;
    }

    protected float CalculateAccelerationTimeFromSpeed(float currentSpeed, float maxSpeed, float totalAccelerationTime)
    {
        float tValue = Mathf.InverseLerp(0, maxSpeed, currentSpeed);
        return tValue * totalAccelerationTime;
    }
    
    /// <summary>
    /// Makes it so that only x and z movement matters in air-movement
    /// </summary>
    protected void ApplyCorrectiveAirForces()
    {
        //Flaws is the use of vector2 which only use x and y, but keep in mind that x => x and z => y
        Vector3 currentVelocity = playerBehaviour.rb.velocity;

        Vector2 currentXZVelocity = new Vector2(currentVelocity.x,currentVelocity.z);
        
        //Works as a cap so the player wont move to fast
        if (currentXZVelocity.magnitude >= playerBehaviour.GetMovementData.GetMidAirForces.GetMaximumSpeed)
        {
            
            Vector2 adaptedXZMoveDir = new Vector2(playerBehaviour.moveDir.x,playerBehaviour.moveDir.z) *
                                playerBehaviour.GetMovementData.GetMidAirForces.GetMaximumSpeed;
            playerBehaviour.rb.velocity = new Vector3(adaptedXZMoveDir.x,currentVelocity.y,adaptedXZMoveDir.y);
            Debug.Log("MaxSpeed - In Air");
        }
        else
        {
            playerBehaviour.rb.AddForce(playerBehaviour.moveDir.normalized * playerBehaviour.GetMovementData.GetMidAirForces.GetAppliedMagnitude, ForceMode.Acceleration);
        }
        
    }


    protected Quaternion UpdateAirborneRotation(Vector2 moveInput, Transform playerTransform, Rigidbody rb)
    {
        Quaternion targetRotation;
        if (moveInput != Vector2.zero)
        {
            targetRotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z));
        }
        else
        {
            targetRotation = Quaternion.LookRotation(new Vector3(playerTransform.forward.x, 0, playerTransform.forward.z));
        }
        //playerBehaviour.transform.rotation = targetRotation;
        return targetRotation;
    }

    protected void UpdateAirborneRotation2(Rigidbody rb, Transform playerTransform, ref float currentVelocity, float smoothTime)
    {
        float targetAngle = MathF.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

        if (playerBehaviour.moveInput != Vector2.zero)
        {
            playerTransform.rotation = Quaternion.Euler(playerTransform.eulerAngles.x, angle, playerTransform.eulerAngles.z);
        }
            

    }

    protected void TakeStep()
    {
        playerBehaviour.GetAudioPort.OnStep(
            playerBehaviour.GetAudioData,
            playerBehaviour.GetCheckerTransform);
    }
    
    /// <summary>
    /// Only capable 4 now to change global parameters (Won't search for a suitable change in any local parameters)
    /// </summary>
    protected void OnEnterChangeGlobalActivityParameter(string parameterName , int value)
    {
        playerBehaviour.GetAudioPort.OnChangeGlobalParameter(parameterName, value);
    }
}
