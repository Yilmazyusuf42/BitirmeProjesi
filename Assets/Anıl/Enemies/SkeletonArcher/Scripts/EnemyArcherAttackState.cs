using UnityEngine;

public class EnemyArcherAttackState : EnemyState
{
    private EnemyArcherSkeleton enemy;
    private int attackIndex;
    private float attackTimer;


    public EnemyArcherAttackState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, EnemyArcherSkeleton enemy)
        : base(stateMachine, enemy, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetZeroVelocity();

        // Pick between attack1 and attack2
        attackIndex = Random.Range(1, 3); // 1 or 2
        enemy.anim.SetFloat("attackType", attackIndex);
        enemy.anim.ResetTrigger("Attack");
        enemy.anim.SetTrigger("Attack");

        enemy.lastAttackTime = Time.time; // ✅ set here for consistency
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
