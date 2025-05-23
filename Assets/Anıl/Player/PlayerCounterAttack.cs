using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttack : PlayerState
{
    public PlayerCounterAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccesfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
        player.anim.SetBool("SuccesfulCounterAttack", false);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<EnemyBase>()!=null)
            {
                if (hit.GetComponent<EnemyBase>().CanBeStunned())
                {
                    stateTimer = .3f;

                    player.anim.SetBool("SuccesfulCounterAttack", true);
                }

            }
        }

        if (stateTimer < 0 || triggerCalled)
        {
            Debug.Log("oldu");
            stateMachine.ChangeState(player.idleState);
        }
           
}
}
