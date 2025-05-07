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
        if (hit.CompareTag("Enemy"))
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // or any value
            }
        }
    }
}

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
