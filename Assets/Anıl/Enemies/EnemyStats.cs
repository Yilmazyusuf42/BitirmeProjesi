using System.Collections;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private EnemyBase m_Enemy;

    [Header("Gold Drop")]
    public Vector2Int goldRange = new Vector2Int(1, 10);

    protected override void Start()
    {
        base.Start();
        m_Enemy = GetComponent<EnemyBase>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

protected override void Die()
{
    base.Die();

        // ✅ Award random gold on death (only once)
        if (GoldManager.instance != null)
        {
            int goldToGive = Random.Range(goldRange.x, goldRange.y + 1);
            GoldManager.instance.AddGold(goldToGive);
    }
        else
        {
            Debug.LogWarning("[EnemyStats] GoldManager instance is missing!");
        }

    m_Enemy.Die();
}

}
