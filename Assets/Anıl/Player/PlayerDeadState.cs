using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    private bool isHandled = false;

    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameState.isPlayerDead = true;
        isHandled = false;

        // Optional: Freeze controls or pause logic
        player.SetZeroVelocity();
        player.anim.SetBool("Die", true);
    }

    public override void Update()
    {
        base.Update();

        // Prevents Update from running indefinitely
        if (isHandled) return;

        player.SetZeroVelocity();

        // Optional: wait for animation to end, or time delay
        if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            // Wait until animation finishes if needed
        }

        // Call death screen (already handled in Player.cs)
        isHandled = true;
    }

    public override void Exit()
    {
        base.Exit();
        GameState.isPlayerDead = false;
        isHandled = false;
        player.anim.SetBool("Die", false);
    }
}
