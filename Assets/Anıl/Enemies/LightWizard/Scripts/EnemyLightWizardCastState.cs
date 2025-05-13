using UnityEngine;

public class EnemyLightWizardCastState : EnemyState
{
    private EnemyLightWizard enemy;
    private int spellIndex;

    public EnemyLightWizardCastState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, EnemyLightWizard enemy)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

public override void Enter()
{
    base.Enter();
    enemy.SetZeroVelocity();
    enemy.FlipTowardsPlayer();

    enemy.anim.SetTrigger("CastLightCharge"); // 🔥 Only cast fireball now
    enemy.SetMagicCooldown();
}


    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        stateMachine.ChangeState(enemy.battleStateInternal);
    }
}
