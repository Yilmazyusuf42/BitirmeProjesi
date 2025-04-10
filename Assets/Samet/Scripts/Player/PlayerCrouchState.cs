using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerState
{
    public PlayerCrouchState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (xInput != 0)
        {
            stateMachine.ChangeState(player.crouchWalkState);
        }
        //else
        //{
        //    stateMachine.ChangeState(player.crouchIdle);
        //}

        if (Input.GetKeyDown(KeyCode.C))
        {
            stateMachine.ChangeState(player.idleState);    
        }
           
        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.crouchAttackState);       
    }
}
