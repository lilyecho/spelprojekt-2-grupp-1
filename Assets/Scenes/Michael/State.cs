using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
        LayerMask layerToIgnore = 1 << 6;
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
        LayerMask layerToIgnore = 1 << 6;
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
        LayerMask layerToIgnore = 1 << 6;
        
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
}
