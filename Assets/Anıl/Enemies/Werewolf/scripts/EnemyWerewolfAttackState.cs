using UnityEngine;

public class EnemyWerewolfAttackState : EnemyState
{
    private EnemyWerewolf enemy;
    private float attackTimer;
    private float attackDuration = 1.1f;

    public EnemyWerewolfAttackState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, EnemyWerewolf enemy)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetZeroVelocity();

        float attackIndex = Mathf.Round(Random.Range(1f, 2.99f));
        enemy.anim.SetFloat("attackType", attackIndex);
        enemy.anim.ResetTrigger("Attack");
        enemy.anim.SetTrigger("Attack");

        enemy.lastAttackTime = Time.time; // ✅ Cooldown timer
        attackTimer = attackDuration;

        Debug.Log($"[SpearSkeletonAttack] attackType {attackIndex}");
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
        stateMachine.ChangeState(enemy.battleState);
    }
}
