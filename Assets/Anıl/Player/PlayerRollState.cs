using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerState
{
    public PlayerRollState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    private float rollSpeed = 9f;
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(rollSpeed * player.facingDir,0);

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
