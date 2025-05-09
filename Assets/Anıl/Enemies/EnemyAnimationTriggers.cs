using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public void AnimationTrigger()
    {
        enemy?.stateMachine?.currentState?.AnimationTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.transform.position,enemy.attackRadius);
        foreach(Collider2D hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().TakeDamage(enemy);
            }
        }
    }
    public void AnimationFinishTrigger()
    {
        enemy?.stateMachine?.currentState?.AnimationFinishTrigger();
    }
}
