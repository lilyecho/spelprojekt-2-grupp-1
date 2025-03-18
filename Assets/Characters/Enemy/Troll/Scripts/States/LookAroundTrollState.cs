using System.Collections;
using System.Collections.Generic;
using Characters.Enemy.Troll.Scripts.States;
using UnityEngine;

public class LookAroundTrollState : TrollStates
{
    public override void Enter()
    {
        TrollBehaviour.stateColor = Color.white;
        TrollBehaviour.activeState = TrollBehaviour.States.LookAround;
        TrollBehaviour.Animator.SetTrigger(TrollBehaviour.lookAroundAP);
    }

    public override void FixedUpdate()
    {
        if (TrollBehaviour.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            TrollBehaviour.Transition(TrollBehaviour.patrolState);
        }
        
        //TrollEyes
        TrollStates newState = Check4Player(TrollBehaviour.GetEyes, TrollBehaviour.GetTrollData.GetTrollSight.range,
            TrollBehaviour.GetTrollData.GetTrollSight.angle);
        if (newState != this)
        {
            TrollBehaviour.Transition(newState);
            return;
        }
            
        //LampEyes
        newState = Check4Player(TrollBehaviour.GetLamp, TrollBehaviour.GetTrollData.GetLampSight.range,TrollBehaviour.GetTrollData.GetLampSight.angle);
        if (newState != this)
        {
            TrollBehaviour.Transition(newState);
            return;
        }
    }
    
    private TrollStates Check4Player(Transform eyes, float range, float angle)
    {
        if (TrollBehaviour.GetTarget == null) return TrollBehaviour.lookAroundState;
        if (!CheckIfPositionIsWalkable(TrollBehaviour.GetTarget.position, range)) return TrollBehaviour.lookAroundState;
        if (!CheckTargetWithinAngleOfSight(eyes, angle)) return TrollBehaviour.lookAroundState;
        if (!CheckIfRaycastHit(eyes, range)) return TrollBehaviour.lookAroundState;

        return TrollBehaviour.chaseState;
    }

    public override void Exit() {}
}
