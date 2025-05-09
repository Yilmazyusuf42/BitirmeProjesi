using UnityEngine;

public class EnemySpearSkeletonAttackState : EnemyState
{
    private EnemySpearSkeleton enemy;
    private float attackTimer;
    private float attackDuration = 1.1f;

    public EnemySpearSkeletonAttackState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, EnemySpearSkeleton enemy)
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
        enemy.anim.ResetTrigger("PlayAttack");
        enemy.anim.SetTrigger("PlayAttack");

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