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
        LayerMask layerToIgnore = 1 << 8;
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


    protected Quaternion AlignToSlope(Transform[] rayCastPoints, Transform playerTransform, Vector3 normal)
    {
        //
        LayerMask layerToIgnore = 1 << 8;
        Vector3 point1 = Vector3.zero;
        Vector3 point2 = Vector3.zero;
        RaycastHit hit1;
        RaycastHit hit2;
        if (Physics.Raycast(rayCastPoints[1].position, Vector3.down, out hit1, 2f, ~layerToIgnore))
        {
            if(Vector3.Angle(Vector3.up, hit1.normal) < 30f)
            {
                point1 = hit1.point;
            }
            
        }
        if (Physics.Raycast(rayCastPoints[2].position, Vector3.down, out hit2, 2f, ~layerToIgnore))
        {
            if (Vector3.Angle(Vector3.up, hit1.normal) < 30f)
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
            targetRotation = Quaternion.FromToRotation(playerTransform.up, normal) * playerTransform.rotation;
        }

        




        //playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, targetRotation, timeCount);

        //timeCount = timeCount + 2 * Time.deltaTime;
        return targetRotation;
    }


    public Vector3 GetSurfaceNormal(Transform[] raycastPoints, float rayCastLength)
    {
        LayerMask layerToIgnore = 1 << 8;
        
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
    
    protected void ApplyCorrectiveAirForces()
    {
        Vector3 currentVelocity = playerBehaviour.rb.velocity;

        Vector2 currentXZVelocity = new Vector2(currentVelocity.x,currentVelocity.z);
        
        //Works as a cap so the player wont move to fast
        if (currentXZVelocity.magnitude >= playerBehaviour.GetMovementData.GetMidAirForces.GetMaximumSpeed)
        {
            Vector2 adaptedXZ = currentXZVelocity.normalized *
                                playerBehaviour.GetMovementData.GetMidAirForces.GetMaximumSpeed;
            playerBehaviour.rb.velocity = new Vector3(adaptedXZ.x,currentVelocity.y,adaptedXZ.y);
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
}
