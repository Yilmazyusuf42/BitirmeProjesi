using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    protected override void Start()
    {
        base.Start();
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

    }
}
