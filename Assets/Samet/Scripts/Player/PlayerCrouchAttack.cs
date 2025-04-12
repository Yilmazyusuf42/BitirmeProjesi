using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchAttack : PlayerState
{
    public PlayerCrouchAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = .1f;
        player.anim.SetBool("Crouch", true);
    }

    public override void Exit()
    {
        base.Exit();
        player.anim.SetBool("Crouch", false);
    }

    public override void Update()
    {
        base.Update();

            player.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.crouchIdle);
    }
}
