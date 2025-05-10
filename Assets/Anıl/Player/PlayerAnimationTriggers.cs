using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
private void AttackTrigger()
{
    Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);

    foreach (var hit in colliders)
    {
        EnemyBase enemy = hit.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.TakeDamage();
        }
    }
}



    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
