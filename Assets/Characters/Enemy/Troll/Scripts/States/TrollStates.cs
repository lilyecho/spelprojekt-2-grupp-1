using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class TrollStates
{
    protected TrollBehaviour TrollBehaviour = null;

    public virtual void Awake(TrollBehaviour trollBehaviour)
    {
        TrollBehaviour = trollBehaviour;
    }
    
    public virtual void Enter(){}
    public virtual void Exit(){}
    public virtual void Update(){}
    public virtual void FixedUpdate(){}
    public virtual void OnValidate() { }
    public virtual void OnDrawGizmos(TrollBehaviour troll) { }

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
        float distance = Vector3.Distance(TrollBehaviour.GetTarget.position, eyes.position);
        return distance <= range;
    }

    /// <summary>
    /// Main axis comes from eyes forward. Matf.abs for only the differance in angles. will calculate according to a rotation axis Vector.up, so the other values only use x and z 
    /// </summary>
    /// <returns></returns>
    protected bool CheckTargetWithinAngleOfSight(Transform eyes, float angleOneSide)
    {
        Vector3 trollPos = eyes.position;
        Vector3 targetPos = TrollBehaviour.GetTarget.position;
        Vector3 directionToPlayer = (new Vector3(targetPos.x,0,targetPos.z) - new Vector3(trollPos.x,0,trollPos.z))
            .normalized;
        float angle = Vector3.Angle(eyes.forward, directionToPlayer);
        
        return MathF.Abs(angle) <= angleOneSide;
    }

    protected bool CheckIfTargetPositionIsWalkable()
    {
        NavMeshPath path = new NavMeshPath();
        bool isWalkable = NavMesh.CalculatePath(TrollBehaviour.GetNavMeshAgent.transform.position,
            TrollBehaviour.GetTarget.position, 1 << NavMesh.GetAreaFromName("Walkable"), path); //Has to do with binary 0,1,2,3 --> 1,2,4,8 1<< x moves the number 1 x ahead

        return isWalkable;
    }
    
    protected bool CheckIfTargetPositionIsWalkable(out NavMeshPath path)
    {
        path = new NavMeshPath();
        bool isWalkable = NavMesh.CalculatePath(TrollBehaviour.GetNavMeshAgent.transform.position,
            TrollBehaviour.GetTarget.position, 1 << NavMesh.GetAreaFromName("Walkable"), path); //Has to do with binary 0,1,2,3 --> 1,2,4,8 1<< x moves the number 1 x ahead

        return isWalkable;
    }

    protected bool CheckIfRaycastHit(Transform eyes,float range)
    {
        LayerMask layerMask = ~LayerMask.GetMask("InteractiveEnvironment", "Ignore Raycast");
        Vector3 directionToPlayer = (TrollBehaviour.GetTarget.position - eyes.position).normalized;
        Physics.Raycast(eyes.position,directionToPlayer ,out RaycastHit hit,range,layerMask);
        
        return hit.collider == TrollBehaviour.GetTarget.GetComponent<Collider>();
    }
}
