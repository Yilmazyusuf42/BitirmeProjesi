using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private EnemyBase enemy;
    private bool flippedRecently = false;

    public EnemyPatrolState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName)
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

    float posX = enemy.transform.position.x;
    float leftBound = enemy.spawnPosition.x - enemy.patrolRange;
    float rightBound = enemy.spawnPosition.x + enemy.patrolRange;

    // 🔁 Flip at patrol bounds
    if (!flippedRecently && (posX <= leftBound && enemy.facingDir < 0 || posX >= rightBound && enemy.facingDir > 0))
    {
        enemy.Flip();
        flippedRecently = true;
    }

    // 🔁 Flip if wall detected
    if (!flippedRecently && enemy.IsWallDetected())
    {
        enemy.Flip();
        flippedRecently = true;
    }

    // ✅ Reset flip cooldown once inside safe zone
    if (posX > leftBound + 0.5f && posX < rightBound - 0.5f && !enemy.IsWallDetected())
    {
        flippedRecently = false;
    }

    // 🧠 Enter combat state if player nearby
    if (enemy.IsPlayerDetected())
    {
        stateMachine.ChangeState(enemy.battleState);
    }

   // Debug.Log($"[Werewolf Patrol] PosX: {enemy.transform.position.x}, Spawn: {enemy.spawnPosition.x}, FacingDir: {enemy.facingDir}, Speed: {enemy.walkSpeed}, VelocityX: {rb.velocity.x}, Wall: {enemy.IsWallDetected()}");

}




}