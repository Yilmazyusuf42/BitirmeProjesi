using UnityEngine;

public class EnemySkeletonAttackState : EnemyState
{
    private EnemySkeleton enemy;
    private float attackTimer;
    private float attackDuration = 1f;

    public EnemySkeletonAttackState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, EnemySkeleton enemy)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

public override void Enter()
{
    base.Enter();

    enemy.SetZeroVelocity();

    float attackIndex = Mathf.Round(Random.Range(1f, 3.99f));
    enemy.anim.SetFloat("attackType", attackIndex);
    enemy.anim.ResetTrigger("Attack");
    enemy.anim.SetTrigger("Attack");

    enemy.lastAttackTime = Time.time; // ✅ start cooldown
    attackTimer = attackDuration;

 //   Debug.Log($"[SkeletonAttack] attackType {attackIndex}");
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
