using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Enemy.Troll.Scripts.States;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class TrollStates
{
    protected TrollBehaviour TrollBehaviour = null;
    
    //Raycast area
    /*protected Vector2[] areaOfRayCasts = new[]
    {
        new Vector2(-0.1f, 0.1f), new Vector2(0,0.1f), new Vector2(0.1f,0.1f),
        new Vector2(-0.1f, 0), new Vector2(0,0), new Vector2(0.1f,0),
        new Vector2(-0.1f, -0.1f), new Vector2(0,-0.1f), new Vector2(0.1f,-0.1f)
    }; */
    
    public virtual void Awake(TrollBehaviour trollBehaviour)
    {
        TrollBehaviour = trollBehaviour;
    }
    
    public virtual void Enter(){}
    public virtual void Exit(){}
    public virtual void Update(){}
    public virtual void FixedUpdate(){}

    public virtual void OnValidate(TrollBehaviour trollBehaviour)
    {
        TrollBehaviour = trollBehaviour;
    }
    public virtual void OnDrawGizmos(TrollBehaviour troll) { }

    /*
    protected IEnumerator Accelerate(float maxSpeed, float totalAccelerationTime)
    {
        while (true)
        {
            float currentSpeed = TrollBehaviour.GetNavMeshAgent.velocity.magnitude;
            float percentage = currentSpeed / maxSpeed;

            float lerpTime = (totalAccelerationTime * percentage+ Time.deltaTime)/totalAccelerationTime;
            if (lerpTime >= 0.99 )
            {
                Debug.Log("Max");
                TrollBehaviour.GetNavMeshAgent.velocity =
                    TrollBehaviour.GetNavMeshAgent.velocity.normalized * maxSpeed;
                TrollBehaviour.GetNavMeshAgent.speed = maxSpeed;
                TrollBehaviour.GetNavMeshAgent.ResetPath();
                break;
            }
            TrollBehaviour.GetNavMeshAgent.velocity =
                (TrollBehaviour.GetNavMeshAgent.destination - TrollBehaviour.transform.position).normalized * Mathf.Lerp(0, maxSpeed,lerpTime < 1 ? lerpTime : 1);
            yield return null;
        }
    }*/
    
    protected void SetUpStateValuesInAgent(StateParameters parameterValues)
    {
        SetAgentSpeed(parameterValues.speed);
        SetAgentAngularSpeed(parameterValues.angularSpeed);
        SetAgentAcceleration(parameterValues.acceleration);
    }
    
    protected void SetAgentSpeed(float speed)
    {
        TrollBehaviour.GetNavMeshAgent.speed = speed;
        TrollBehaviour.Animator.SetFloat(TrollBehaviour.speedAP, speed);
    }
    protected void SetAgentAngularSpeed(float speed)
    {
        TrollBehaviour.GetNavMeshAgent.angularSpeed = speed;
    }
    protected void SetAgentAcceleration(float speed)
    {
        TrollBehaviour.GetNavMeshAgent.acceleration = speed;
    }
    
    protected bool CheckTargetInRange(Transform eyes,float range)
    {
        float distance = Vector3.Distance(TrollBehaviour.GetTargetTransform.position, eyes.position);
        return distance <= range;
    }

    /// <summary>
    /// Main axis comes from eyes forward. Matf.abs for only the differance in angles. will calculate according to a rotation axis Vector.up, so the other values only use x and z 
    /// </summary>
    /// <returns></returns>
    protected bool CheckTargetWithinAngleOfSight(Transform eyes, float angleOneSide)
    {
        Vector3 trollPos = eyes.position;
        Vector3 targetPos = TrollBehaviour.GetTargetTransform.position;
        Vector3 directionToPlayer = (new Vector3(targetPos.x,0,targetPos.z) - new Vector3(trollPos.x,0,trollPos.z))
            .normalized;
        float angle = Vector3.Angle(TrollBehaviour.transform.forward, directionToPlayer);
        
        return MathF.Abs(angle) <= angleOneSide;
    }

    public bool CheckIfPositionIsWalkable(Vector3 position, float range)
    {
        Physics.Raycast(position + Vector3.up, Vector3.down,out RaycastHit hit);
        /*bool isWalkable = NavMesh.CalculatePath(TrollBehaviour.GetNavMeshAgent.transform.position,
            hit.point, 1 << NavMesh.GetAreaFromName("Walkable"), path); //Has to do with binary 0,1,2,3 --> 1,2,4,8 1<< x moves the number 1 x ahead*/

        bool isWalkable = NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, range,
            1 << NavMesh.GetAreaFromName("Walkable"));
        
        return isWalkable;
    }
    
    protected bool CalculatePath(Vector3 position,out NavMeshPath path)
    {
        path = new NavMeshPath();
        bool isWalkable = NavMesh.CalculatePath(TrollBehaviour.GetNavMeshAgent.transform.position,
            position, 1 << NavMesh.GetAreaFromName("Walkable"), path); //Has to do with binary 0,1,2,3 --> 1,2,4,8 1<< x moves the number 1 x ahead
        
        return isWalkable;
    }

    protected bool CheckIfPlayerHidden()
    {
        return TrollBehaviour.GetTarget.Hidden;
    }
    
    /// <summary>
    /// If true the it has hit the player
    /// </summary>
    /// <param name="eyes"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    protected bool CheckIfRaycastHit(Transform eyes,float range)
    {
        LayerMask layerMask = ~LayerMask.GetMask("InteractiveEnvironment", "Ignore Raycast");
        Transform target = TrollBehaviour.GetTargetTransform;
        
        Vector3 directionToPlayer = (target.position+new Vector3(0,0.2f,0)  - eyes.position).normalized;
        Physics.Raycast(eyes.position,directionToPlayer ,out RaycastHit hit,range,layerMask);
            
        foreach (var playerCollider in TrollBehaviour.GetTargetTransform.GetComponents<Collider>())
        {
            if (hit.collider == playerCollider)
            {
                return true;
            }
        }
        return false;
    }
}
