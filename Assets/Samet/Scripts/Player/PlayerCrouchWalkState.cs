using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchWalkState : PlayerCrouchState
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

        player.SetVelocity(xInput * player.moveSpeed * player.crouchSpeed, rb.velocity.y);  

    }
}
