using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private Enemy enemy;
    private bool flippedRecently = false;

    public EnemyPatrolState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemyBase;
    }

    public override void Enter()
    {
        base.Enter();
        flippedRecently = false;

        // ✅ Force visual facing to match patrol direction
        enemy.transform.localScale = new Vector3(enemy.facingDir, 1f, 1f);

        enemy.anim.SetBool("PlayWalk", true);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.anim.SetBool("PlayWalk", false);
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.walkSpeed * enemy.facingDir, rb.velocity.y);

        float delta = enemy.transform.position.x - enemy.spawnPosition.x;

    // Flip if enemy is out of patrol range AND going further away
        if (!flippedRecently && Mathf.Abs(delta) >= enemy.patrolRange)
        {
            if ((delta > 0 && enemy.facingDir > 0) || (delta < 0 && enemy.facingDir < 0))
            {
                enemy.Flip();
                flippedRecently = true;
            }
        }

    // Reset flip cooldown once safely inside patrol area
    if (Mathf.Abs(delta) < enemy.patrolRange * 0.8f)
        {
            flippedRecently = false;
        }

        if (enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

}