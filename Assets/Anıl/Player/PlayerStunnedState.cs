using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunnedState : PlayerState
{
    private float stunTime;
    private float startTime;

    private float originalGravity;
    private RigidbodyType2D originalBodyType;

    public PlayerStunnedState(Player player, PlayerStateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName)
    {
    }

    public void SetStunTime(float duration)
    {
        stunTime = duration;
    }

    public override void Enter()
    {
        base.Enter();
        startTime = Time.time;
        player.SetVelocity(0f, 0f);

        // Freeze physics
        if (player.rb != null)
        {
            originalGravity = player.rb.gravityScale;
            originalBodyType = player.rb.bodyType;

            player.rb.velocity = Vector2.zero;
            player.rb.gravityScale = 0f;
            player.rb.bodyType = RigidbodyType2D.Kinematic;
        }

        player.anim.SetBool("Stunned", true); // Optional
    }

    public override void Exit()
    {
        base.Exit();

        // Restore physics
        if (player.rb != null)
        {
            player.rb.gravityScale = originalGravity;
            player.rb.bodyType = originalBodyType;
        }

        player.anim.SetBool("Stunned", false); // Optional
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0f, 0f); // Lock movement manually

        if (Time.time >= startTime + stunTime)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
