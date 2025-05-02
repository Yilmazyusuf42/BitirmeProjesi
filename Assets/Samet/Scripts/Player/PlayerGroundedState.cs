using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState: PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (Input.GetKeyDown(KeyCode.Q))
            stateMachine.ChangeState(player.counterAttack);

        if (Input.GetKeyDown(KeyCode.C))
        {
            player.SetZeroVelocity();
            player.anim.SetBool("Crouch", true);
            stateMachine.ChangeState(player.crouchIdle);
        }
            
        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);

        if (Input.GetKeyDown(KeyCode.R)&&SkillManager.instance.roll.CanUseSkill())
            stateMachine.ChangeState(player.rollState);

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }
}
