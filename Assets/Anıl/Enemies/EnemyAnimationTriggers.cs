using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    private EnemyBase enemyBase;

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
    // For EnemyMelee
    if (enemyBase is EnemyMelee melee && melee.attackCheck != null)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            melee.attackCheck.position,
            melee.attackRadius
        );

        foreach (Collider2D hit in colliders)
        {
            if (hit.TryGetComponent(out Player player))
            {
                player.TakeDamage(enemyBase);
            }
        }
        return;
    }

    // For EnemyHybrid
    if (enemyBase is EnemyHybrid hybrid && hybrid.attackCheck != null)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            hybrid.attackCheck.position,
            hybrid.attackRadius
        );

        foreach (Collider2D hit in colliders)
        {
            if (hit.TryGetComponent(out Player player))
            {
                player.TakeDamage(enemyBase);

                if (hit.TryGetComponent(out Player player))
                {
                    player.TakeDamage(enemyBase, true); // Pass EnemyBase reference
                }

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
