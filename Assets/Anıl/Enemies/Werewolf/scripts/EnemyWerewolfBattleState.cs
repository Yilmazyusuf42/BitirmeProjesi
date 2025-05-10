using UnityEngine;

public class EnemyWerewolfBattleState : EnemyState
{
    private EnemyWerewolf enemy;

    public EnemyWerewolfBattleState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, EnemyWerewolf enemy)
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

    enemy.FlipTowardsPlayer();
    float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.player.position);

    if (distanceToPlayer <= enemy.minAgroRange)
    {
        enemy.SetZeroVelocity();
        enemy.anim.SetBool("PlayRun", false); // ✅ idle during cooldown

        if (enemy.CanAttack())
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        return;
    }

    if (enemy.IsPlayerDetected())
    {
        enemy.SetVelocity(enemy.runSpeed * enemy.facingDir, rb.velocity.y);
        enemy.anim.SetBool("PlayRun", true);
    }
    else
    {
        stateMachine.ChangeState(enemy.moveState);
    }
}


}
