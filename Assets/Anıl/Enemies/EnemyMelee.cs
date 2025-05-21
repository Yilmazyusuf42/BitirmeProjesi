using UnityEngine;

public class EnemyMelee : EnemyBase
{
    [Header("Melee Attack Info")]
    public Transform attackCheck;
    public float attackRadius;

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
#if UNITY_EDITOR
        if (attackCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
        }
#endif
    }
}
