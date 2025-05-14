using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    private EnemyBase enemyBase;

    public bool isPhysicalDamage = true;


    private void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
    }

    // Called by animation event to notify current state
    public void AnimationTrigger()
    {
        enemyBase?.stateMachine?.currentState?.AnimationTrigger();
    }

    // Called by melee attack animation to trigger damage
private void AttackTrigger()
{
    if (enemyBase is EnemyMelee meleeEnemy && meleeEnemy.attackCheck != null)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            meleeEnemy.attackCheck.position,
            meleeEnemy.attackRadius
        );

        foreach (Collider2D hit in colliders)
        {
            if (hit.TryGetComponent(out Player player))
            {
                    enemyBase.stats.DoDamage(player.stats, isPhysicalDamage);
            }
        }
    }
    else if (enemyBase is EnemyHybrid hybridEnemy && hybridEnemy.attackCheck != null)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            hybridEnemy.attackCheck.position,
            hybridEnemy.attackRadius
        );

        foreach (Collider2D hit in colliders)
        {
            if (hit.TryGetComponent(out Player player))
            {
                enemyBase.stats.DoDamage(player.stats, isPhysicalDamage);
            }
        }
    }
}





    // Called by animation event to signal animation is done
    public void AnimationFinishTrigger()
    {
        enemyBase?.stateMachine?.currentState?.AnimationFinishTrigger();
    }
}
