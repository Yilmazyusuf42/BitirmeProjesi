using UnityEngine;

public class EnemyWerewolfAttackState : EnemyState
{
    private EnemyWerewolf enemy;
    private int attackIndex;

    private float attackDuration = 1.2f; // ⏱ fallback in case AnimationFinishTrigger doesn't fire
    private float attackTimer;

    public EnemyWerewolfAttackState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, EnemyWerewolf enemy)
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

    enemy.lastAttackTime = Time.time; // ✅ cooldown starts here
    attackTimer = attackDuration;

    Debug.Log($"[AttackState] Started attackType {attackIndex} on {enemy.name}");
}



    public override void Update()
    {
        base.Update();

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            Debug.LogWarning($"[AttackState] Fallback timeout on {enemy.name} → returning to battle.");
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        Debug.Log($"[AttackState] AnimationFinishTrigger called on {enemy.name} → back to battle.");

        if (stateMachine != null && enemy.battleState != null)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
        else
        {
            Debug.LogError("[AttackState] ERROR: Cannot transition. Missing stateMachine or battleState.");
        }
    }
}
