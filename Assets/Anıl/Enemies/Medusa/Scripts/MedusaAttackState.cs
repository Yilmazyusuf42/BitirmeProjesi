using UnityEngine;

public class MedusaAttackState : EnemyState
{
    private Medusa enemy;
    private float attackTimer;
    private float attackDuration = 1f;

    public MedusaAttackState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, Medusa enemy)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

public override void Enter()
{
    base.Enter();

    enemy.SetZeroVelocity();

    // ✅ Check if Medusa can use Snake Attack
    if (enemy.CanUseSnakeAttack())
    {
        enemy.UseSnakeAttack(); // start cooldown
        stateMachine.ChangeState(enemy.snakeAttackState); // switch to dedicated snake logic
        return;
    }

    // 🔁 Otherwise, do regular random attack
    float attackIndex = Mathf.Round(Random.Range(1f, 2.99f)); // Skip Attack 1
    enemy.anim.SetFloat("attackType", attackIndex);
    enemy.anim.ResetTrigger("Attack");
    enemy.anim.SetTrigger("Attack");

    enemy.lastAttackTime = Time.time;
    attackTimer = attackDuration;
}



    public override void Update()
    {
        base.Update();

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
       // Debug.Log("Tetiklendi");
        stateMachine.ChangeState(enemy.battleState);
    }
}
