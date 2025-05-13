using UnityEngine;

public class EnemyFireWizardCastState : EnemyState
{
    private EnemyFireWizard enemy;
    private int spellIndex;

    public EnemyFireWizardCastState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, EnemyFireWizard enemy)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

public override void Enter()
{
    base.Enter();
    enemy.SetZeroVelocity();
    enemy.FlipTowardsPlayer();

    enemy.anim.SetTrigger("CastFireball"); // 🔥 Only cast fireball now
    enemy.SetMagicCooldown();
}


    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        stateMachine.ChangeState(enemy.battleStateInternal);
    }
}
