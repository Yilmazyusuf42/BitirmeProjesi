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

        float distanceFromSpawn = Mathf.Abs(enemy.transform.position.x - enemy.spawnPosition.x);

        // ✅ Flip once if out of range or blocked
        if (!flippedRecently &&
            (distanceFromSpawn >= enemy.patrolRange || enemy.IsWallDetected() || !enemy.IsGroundDetected()))
        {
            enemy.Flip();
            flippedRecently = true;
        }

        // ✅ Allow future flips when safely inside patrol range again
        if (distanceFromSpawn < enemy.patrolRange * 0.8f)
        {
            flippedRecently = false;
        }

        // ✅ Detect player
        if (enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}