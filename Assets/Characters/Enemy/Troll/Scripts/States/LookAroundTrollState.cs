using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundTrollState : SearchStateTroll
{
    public override void Enter()
    {
        TrollBehaviour.Animator.SetTrigger(TrollBehaviour.lookAroundAP);
    }

    public override void FixedUpdate()
    {
        Debug.Log(TrollBehaviour.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (TrollBehaviour.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            TrollBehaviour.Transition(TrollBehaviour.patrolState);
        }
        
        //TrollEyes
        if (Check4Player(TrollBehaviour.GetEyes, TrollBehaviour.GetTrollData.GetTrollSight.range,
                TrollBehaviour.GetTrollData.GetTrollSight.angle)) 
        {
            TrollBehaviour.Transition(TrollBehaviour.chaseState);
            return;
        }
        
        //Lampeyes
        if (Check4Player(TrollBehaviour.GetLamp, TrollBehaviour.GetTrollData.GetLampSight.range,
                TrollBehaviour.GetTrollData.GetLampSight.angle))
        {
            TrollBehaviour.Transition(TrollBehaviour.chaseState);
            return;
        }
    }

    public override void Exit() {}
}
