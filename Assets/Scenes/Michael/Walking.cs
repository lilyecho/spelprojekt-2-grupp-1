using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Walking : State
{
    public Walking(PlayerBehaviour playerBehaviour) : base(playerBehaviour)
    {

    }

    float time = 0f;
    Vector3 normal;
    public override void Enter()
    {
        Debug.Log("WALKING");
        playerBehaviour.moveSpeed = playerBehaviour.walkSpeed;
    }
    public override void Exit()
    {

    }

    public override void OnCollision(Collision collision)
    {

    }

    public override void Update()
    {
        playerBehaviour.RotateCharacter(playerBehaviour.moveDir);
        normal = GetSurfaceNormal(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength);
        //playerBehaviour.transform.rotation = AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, time, Vector3.up);
        playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, AlignToSlope(playerBehaviour.rayCastPoints, playerBehaviour.transform, normal), time) ;
        time = time + Time.deltaTime;
        if (!CheckForGround(playerBehaviour.rayCastPoints, playerBehaviour.rayCastLength))
        {
            playerBehaviour.ChangeState(playerBehaviour.falling);
        }
    }

    public override void FixedUpdate()
    {
        playerBehaviour.rb.AddForce(-normal * 9.81f, ForceMode.Acceleration);
        playerBehaviour.moveDir = Vector3.ProjectOnPlane(playerBehaviour.moveDir, normal).normalized;
        playerBehaviour.rb.velocity = playerBehaviour.moveDir.normalized * playerBehaviour.moveSpeed;

    }

    public override void OnSpaceBar(InputAction.CallbackContext context)
    {

    }
    public override void OnShift(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerBehaviour.ChangeState(playerBehaviour.sneaking);
        }
    }
    public override void OnCTRL(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            playerBehaviour.ChangeState(playerBehaviour.running);
        }
    }

    public override void OnWASD(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerBehaviour.ChangeState(playerBehaviour.idle);
        }
    }
    public override void OnMOUSE(InputAction.CallbackContext context)
    {

    }
    /*
    public bool CheckForGround()
    {
        foreach (Transform t in playerBehaviour.rayCastPoints)
        {
            if (Physics.Raycast(t.position, Vector3.down, playerBehaviour.rayCastLength))
            {
                return true;
            }
        }
        return false;
    }
    */
}

/*
public override void Update()
{
    if (!CheckForGround())
    {
        playerBehaviour.ChangeState(playerBehaviour.falling);
    }
    rotatedTiltVector = Vector3.Cross(dir, Vector3.right).normalized;
    onSlope = CheckForSlope();
    normal = GetSurfaceNormal();
    playerBehaviour.RotateCharacter(playerBehaviour.moveDir);
    AlignToSlope();
    if (onSlope)
    {
        //playerBehaviour.rb.useGravity = false;
        //playerBehaviour.rb.AddForce(-normal * 9.81f, ForceMode.Acceleration);

        //playerBehaviour.moveDir = Vector3.ProjectOnPlane(playerBehaviour.moveDir, normal).normalized;


    }
}


public override void FixedUpdate()
{
    if (onSlope)
    {
        playerBehaviour.rb.AddForce(-normal * 9.81f, ForceMode.Acceleration);

        playerBehaviour.moveDir = Vector3.ProjectOnPlane(playerBehaviour.moveDir, rotatedTiltVector).normalized;
        playerBehaviour.rb.velocity = playerBehaviour.moveDir.normalized * playerBehaviour.moveSpeed;
    }
    else
    {
        playerBehaviour.moveDir = Vector3.ProjectOnPlane(playerBehaviour.moveDir, rotatedTiltVector).normalized;
        playerBehaviour.rb.velocity = playerBehaviour.moveDir.normalized * playerBehaviour.moveSpeed;
    }
}

public override void OnSpaceBar(InputAction.CallbackContext context)
{

}
public override void OnShift(InputAction.CallbackContext context)
{
    if (context.performed)
    {
        playerBehaviour.ChangeState(playerBehaviour.sneaking);
    }
}
public override void OnCTRL(InputAction.CallbackContext context)
{
    if (context.performed)
    {
        playerBehaviour.ChangeState(playerBehaviour.running);
    }
}

public override void OnWASD(InputAction.CallbackContext context)
{
    if (context.canceled)
    {
        playerBehaviour.ChangeState(playerBehaviour.idle);
    }
}
public override void OnMOUSE(InputAction.CallbackContext context)
{

}

public bool CheckForGround()
{
    foreach (Transform t in playerBehaviour.rayCastPoints)
    {
        if (Physics.Raycast(t.position, Vector3.down, 0.6f))
        {
            return true;
        }
    }
    return false;
}



bool onSlope = false;
public bool CheckForSlope()
{
    LayerMask layerToIgnore = 1 << 6;
    foreach (Transform t in playerBehaviour.rayCastPoints)
    {
        RaycastHit hit;
        if (Physics.Raycast(t.position, Vector3.down, out hit, 0.6f, ~layerToIgnore))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);

            if (angle < 30 && angle > 0.1f)
            {
                //Debug.Log("SLOPE HIT");
                return true;
            }
        }
    }
    return false;
}

Vector3 normal;

public Vector3 GetSurfaceNormal()
{
    LayerMask layerToIgnore = 1 << 6;
    foreach (Transform t in playerBehaviour.rayCastPoints)
    {
        
        RaycastHit hit;
        if (Physics.Raycast(t.position, Vector3.down, out hit, 0.6f, ~layerToIgnore))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);

            if (angle < 30 && angle > 0.1f)
            {
                //Debug.Log("SLOPE HIT");
                return hit.normal;
            }
        }
        


    }
    RaycastHit hit;
    if (Physics.Raycast(playerBehaviour.rayCastPoints[1].position, Vector3.down, out hit, 0.6f, ~layerToIgnore))
    {
        float angle = Vector3.Angle(Vector3.up, hit.normal);


        Debug.Log("SLOPE HIT");
        return hit.normal;
    }
    else if (Physics.Raycast(playerBehaviour.rayCastPoints[0].position, Vector3.down, out hit, 0.6f, ~layerToIgnore))
    {
        return hit.normal;
    }
    else if (Physics.Raycast(playerBehaviour.rayCastPoints[2].position, Vector3.down, out hit, 0.6f, ~layerToIgnore))
    {
        return hit.normal;
    }

    return Vector3.up;
}



Vector3 dir;
Vector3 rotatedTiltVector;
private float timeCount = 0.0f;
void AlignToSlope()
{
    //
    LayerMask layerToIgnore = 1 << 6;
    Vector3 point1 = new Vector3();
    Vector3 point2 = new Vector3();
    RaycastHit hit1;
    RaycastHit hit2;
    if (Physics.Raycast(playerBehaviour.rayCastPoints[1].position, Vector3.down, out hit1, 2f, ~layerToIgnore))
    {
        point1 = hit1.point;
    }
    if (Physics.Raycast(playerBehaviour.rayCastPoints[2].position, Vector3.down, out hit2, 2f, ~layerToIgnore))
    {
        point2 = hit2.point;
    }
    
    bool point1hit = Physics.Raycast(playerBehaviour.rayCastPoints[1].position, Vector3.down, out hit1, 2f, ~layerToIgnore);
    bool point2hit = Physics.Raycast(playerBehaviour.rayCastPoints[2].position, Vector3.down, out hit2, 2f, ~layerToIgnore);
    
    Quaternion targetRotation;
    Vector3 vectorBetweenPoints;
    if (point1 != null && point2 != null)
    {
        vectorBetweenPoints = (point1 - point2).normalized;
        targetRotation = Quaternion.FromToRotation(playerBehaviour.transform.forward, vectorBetweenPoints) * playerBehaviour.transform.rotation;
        dir = vectorBetweenPoints;
    }
    else
    {
        targetRotation = Quaternion.FromToRotation(playerBehaviour.transform.up, normal) * playerBehaviour.transform.rotation;
    }
    
    Quaternion targetRotation = Quaternion.FromToRotation(playerBehaviour.transform.up, normal)  * playerBehaviour.transform.rotation ;




    playerBehaviour.transform.rotation = Quaternion.Slerp(playerBehaviour.transform.rotation, targetRotation, timeCount);

    timeCount = timeCount + 2 * Time.deltaTime;
}
*/
