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

    int attackIndex;

    // Keep choosing until it's different from the last
    do
    {
        attackIndex = Mathf.RoundToInt(Random.Range(1f, 3.99f)); // 1 to 3 inclusive
    }
    while (attackIndex == enemy.lastAttackIndex);

    enemy.lastAttackIndex = attackIndex; // store new one

    enemy.anim.SetFloat("attackType", attackIndex);
    enemy.anim.ResetTrigger("Attack");
    enemy.anim.SetTrigger("Attack");

    enemy.lastAttackTime = Time.time;
    attackTimer = attackDuration;

    Debug.Log($"[FireWizardAttack] attackType {attackIndex}");
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
