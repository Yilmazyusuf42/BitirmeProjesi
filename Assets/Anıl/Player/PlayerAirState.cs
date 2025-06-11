using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private float fallTime;
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        fallTime = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        fallTime += Time.deltaTime;

        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlide);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
            player.SetZeroVelocity();
            if (fallTime > 1)
            {
                player.stats.TakeDamage(Mathf.RoundToInt(Mathf.Round(fallTime * 15)));
                player.entityFx?.Flash();
                Debug.Log(fallTime);
            }
            if (fallTime > 10)
            {
                player.Die();
            }
        }
           

        

        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);


        if (Input.GetKeyDown(KeyCode.Space) && !player.IsGroundDetected())
            SkillManager.instance.doubleJump.CanUseSkill();
    }
}
