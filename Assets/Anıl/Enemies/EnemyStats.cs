using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private EnemyBase m_Enemy;
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

        m_Enemy.Die();
    }
}
