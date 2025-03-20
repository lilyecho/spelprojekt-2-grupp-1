using FMODUnity;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class State
{
    protected PlayerBehaviour playerBehaviour;

    public UnityEvent OnEnter;
    
    public UnityEvent OnExit;

    protected const float InternalGravity = 9.82f;

    public virtual void Awake(PlayerBehaviour player)
    {
        playerBehaviour = player;
    }
    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void OnCollision(Collision collision) {}
    public virtual void Update() {}
    public virtual void FixedUpdate() {}
    public virtual void OnDrawGizmos(PlayerBehaviour player) {}
    public virtual void OnDrawGizmosSelected(PlayerBehaviour player) {}
    public virtual void OnSpaceBar(InputAction.CallbackContext context) {}
    public virtual void OnShift(InputAction.CallbackContext context) {}
    public virtual void OnCTRL(InputAction.CallbackContext context) {}
    public virtual void OnWASD(InputAction.CallbackContext context) {}
    public virtual void OnShrink(InputAction.CallbackContext context) {}
    public virtual void OnMOUSE(InputAction.CallbackContext context) {}
    public virtual void OnValidate(PlayerBehaviour player) {}
    
    protected bool CheckForGround(Transform[] raycastPoints, float rayCastLength)
    {
        LayerMask layerToIgnore = (1 << 8) | (1 << 2) | (1 << 10);
        RaycastHit hit;
        foreach (Transform t in raycastPoints)
        {
            if (Physics.Raycast(t.position, Vector3.down, out hit, rayCastLength, ~layerToIgnore))
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                
                if (angle < playerBehaviour.PlayerData.GetMaxRotationAngle)
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
        LayerMask layerToIgnore = (1 << 8) | (1 << 2) | (1 << 10);
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
        float targetXAngle = 0f;
        //float smoothedXAngle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.x, targetXAngle, ref currentXVelocity, 0.1f);
        Quaternion targetRotation;
        Vector3 vectorBetweenPoints;
        if (point1 != Vector3.zero && point2 != Vector3.zero)
        {
            vectorBetweenPoints = (point1 - point2).normalized;

            targetXAngle = Vector3.SignedAngle(new Vector3(playerBehaviour.transform.forward.x, 0, playerBehaviour.transform.forward.z).normalized , vectorBetweenPoints, playerTransform.right);
            //targetRotation = Quaternion.FromToRotation(playerTransform.forward, vectorBetweenPoints) * playerTransform.rotation;

            targetRotation = Quaternion.Euler(targetXAngle, 0, 0);
        }
        else
        {
            //targetRotation = Quaternion.FromToRotation(playerTransform.up, normal) * playerTransform.rotation;
            //targetRotation = Quaternion.FromToRotation(playerTransform.up, Vector3.up)/* * playerTransform.rotation*/;
            targetRotation = Quaternion.Euler(playerTransform.rotation.x, 0, 0); ;
        }

        
        //playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, targetRotation, timeCount);

        //timeCount = timeCount + 2 * Time.deltaTime;
        return targetRotation;
    }

    protected void AlignToSlope2(Transform[] rayCastPoints, Transform playerTransform, float slopeCheckerLength, float maxRotationAngle, ref float currentVelocity, float smoothTime)
    {
        //
        LayerMask layerToIgnore = (1 << 8) | (1 << 2) | (1 << 10);
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
        LayerMask layerToIgnore = (1 << 8) | (1 << 2) | (1 << 10);
        
        RaycastHit hit;
        if (Physics.Raycast(raycastPoints[1].position, Vector3.down, out hit, rayCastLength, ~layerToIgnore))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);

            if (angle < playerBehaviour.PlayerData.GetMaxRotationAngle)
            {
                return hit.normal;
            }
            
        }
        else if (Physics.Raycast(raycastPoints[0].position, Vector3.down, out hit, rayCastLength, ~layerToIgnore))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);

            if (angle < playerBehaviour.PlayerData.GetMaxRotationAngle)
            {
                return hit.normal;
            }
        }
        else if (Physics.Raycast(raycastPoints[2].position, Vector3.down, out hit, rayCastLength, ~layerToIgnore))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);

            if (angle < playerBehaviour.PlayerData.GetMaxRotationAngle)
            {
                return hit.normal;
            }
        }

        //return Vector3.up;
        return Vector3.zero;
    }

    private float CalculateNextSpeed(float maxSpeed, float currentAccTime, float totalAccelerationTime)
    {
        if (playerBehaviour.rb.velocity.magnitude >= maxSpeed)
        {
            //Debug.Log("MaxSpeed");
            return maxSpeed;
        }
        
        float tValue = Mathf.Clamp(currentAccTime / totalAccelerationTime,0,1);
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
    protected void ApplyCorrectiveAirForces(AirForceMode airForceMode)
    {
        if (playerBehaviour.moveDir == Vector3.zero) return;

        MidAirForces midAirForces = GetMidAirForce(airForceMode);
        
        //Flaws is the use of vector2 which only use x and y, but keep in mind that x => x and z => y
        Vector3 currentVelocity = playerBehaviour.rb.velocity;

        //Works as a cap so the player wont move to fast
        float currentXZSpeed = new Vector2(currentVelocity.x,currentVelocity.z).magnitude;
        if (currentXZSpeed >= midAirForces.GetMaximumSpeed)
        {
            Vector3 forceDir = new Vector3(playerBehaviour.moveDir.x,0,playerBehaviour.moveDir.z).normalized;
            playerBehaviour.rb.AddForce(forceDir * midAirForces.GetAppliedMagnitude, ForceMode.Acceleration);

            Vector3 newVelocity = new Vector3(playerBehaviour.rb.velocity.x, 0,playerBehaviour.rb.velocity.z).normalized* midAirForces.GetMaximumSpeed;
            newVelocity.y = playerBehaviour.rb.velocity.y;
            playerBehaviour.rb.velocity = newVelocity;
            
        }
        else
        {
            Vector3 forceDir = new Vector3(playerBehaviour.moveDir.x,0,playerBehaviour.moveDir.z).normalized;
            playerBehaviour.rb.AddForce(forceDir * midAirForces.GetAppliedMagnitude, ForceMode.Acceleration);
        }
    }

    private MidAirForces GetMidAirForce(AirForceMode airForceMode)
    {
        switch (airForceMode)
        {
            case AirForceMode.Sneak:
                return playerBehaviour.PlayerData.GetAirForceSneak;
            case AirForceMode.Walk:
                return playerBehaviour.PlayerData.GetAirForceWalk;
            case AirForceMode.Run:
                return playerBehaviour.PlayerData.GetAirForceRun;
            case AirForceMode.Glide:
                return playerBehaviour.PlayerData.GetAirForceGlide;
            default:
                throw new ArgumentException("Missing airForceMode implementation for airCorrectiveForces");
        }
    }

    protected AirForceMode ConvertMovementModeToAirForceMode(PlayerBehaviour.MovementMode movementMode)
    {
        switch (movementMode)
        {
            case PlayerBehaviour.MovementMode.SNEAK:
                return AirForceMode.Sneak;
            case PlayerBehaviour.MovementMode.WALK:
                return AirForceMode.Walk;
            case PlayerBehaviour.MovementMode.RUN:
                return AirForceMode.Run;
            default:
                throw new ArgumentException("Missing movement mode implementation for conversion to AirForceMode");
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
    

    /// <summary>
    /// Transform up-vector is world-direction (0,1,0)
    /// </summary>
    protected void ChangeRotationToStandard()
    {
        Vector3 forward = new Vector3(playerBehaviour.transform.forward.x, 0, playerBehaviour.transform.forward.z)
            .normalized;
        playerBehaviour.transform.rotation = Quaternion.LookRotation(forward, new Vector3(0,1,0));
    }


    protected bool Glide()
    {
        if (!CheckForGround(playerBehaviour.rayCastPoints, playerBehaviour.PlayerData.GetGlideMinimumHeight) && playerBehaviour.GetAbilities.HasFlag(AbilityData.Abilities.Glide)
            && EnemyManager.instance.GetClosestDistanceToEnemyFromPlayer(out float? dist) && dist > playerBehaviour.minDistToTroll)
        {
            return true;
        }
        return false;
    }
    
    protected bool ExitGlide()
    {
        if (CheckForGround(playerBehaviour.rayCastPoints, playerBehaviour.PlayerData.GetGlideExitHeight))
        {
            return true;
        }
        return false;
    }

    protected void CreateSoundAlert(Vector3 position, float range)
    {
        SoundAlertInfo soundAlertInfo = new SoundAlertInfo
        {
            soundRange = range,
            point = position
        };
            
        SoundAlertCreation.CreateAlertPoint(soundAlertInfo,playerBehaviour.EnemyManagerPort);
    }



    protected void ApplyHorizontalCounterForce()
    {
        Vector2 horizontalMagnitude = new Vector2(playerBehaviour.rb.velocity.x, playerBehaviour.rb.velocity.z);
        if (playerBehaviour.moveInput == Vector2.zero && horizontalMagnitude.magnitude > 0.001f)
        {
            playerBehaviour.rb.AddForce(new Vector3(playerBehaviour.rb.velocity.x, 0, playerBehaviour.rb.velocity.z).normalized * (-1 * 10), ForceMode.Force);
        }
    }
    
    public enum AirForceMode
    {
        None,
        Sneak,
        Walk,
        Run,
        Glide
    }

}
