using UnityEngine;

public class EnemyWerewolfBattleState : EnemyState
{
    private EnemyWerewolf enemy;

    public EnemyWerewolfBattleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, EnemyWerewolf enemy)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool("PlayRun", true); // ✅ Start run animation
    }

    public override void Exit()
    {
        base.Exit();
        enemy.anim.SetBool("PlayRun", false); // ✅ Stop run animation
    }

    public override void Update()
    {
        base.Update();

        // Face and chase player
        enemy.FlipTowardsPlayer();
        enemy.SetVelocity(enemy.runSpeed * enemy.facingDir, rb.velocity.y);

        if (enemy.IsPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(enemy.attackState);
            return;
        }

        if (!enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.moveState);
            return;
        }
    }
}
