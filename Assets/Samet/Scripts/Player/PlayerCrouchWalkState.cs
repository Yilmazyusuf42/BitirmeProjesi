using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchWalkState : PlayerState
{
    public PlayerCrouchWalkState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput == 0)
        {
            stateMachine.ChangeState(player.crouchState);
        }
        else
        {
            player.SetVelocity(xInput * player.moveSpeed*player.crouchSpeed, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            player.SetZeroVelocity();
            stateMachine.ChangeState(player.idleState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            player.SetZeroVelocity();
            stateMachine.ChangeState(player.crouchAttackState);
        }
       

    }
}
