using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTrigger : MonoBehaviour
{
    Enemy enemy => GetComponentInParent<Enemy>();
    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders =Physics2D.OverlapCircleAll(enemy.attackCheck.position,enemy.attackRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().Damage();
            }
        }
    }
    protected void OpenCounterAttackWindow()=> enemy.OpenAttackCounterWindow();
    protected void CloseCounterAttackWindow()=>enemy.CloseAttackCounterWindow();
}
