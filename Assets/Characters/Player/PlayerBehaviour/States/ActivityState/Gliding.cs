using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gliding : State
{
    public Gliding(PlayerBehaviour playerBehaviour) : base(playerBehaviour) {}

    public override void Enter()
    {
        playerBehaviour.ChangeState(playerBehaviour.falling);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
