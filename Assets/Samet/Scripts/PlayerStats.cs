using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

public override void TakeDamage(int _damage)
{
    base.TakeDamage(_damage);
    Debug.Log($"[PlayerStats] Took {_damage} damage.");
}

protected override void Die()
{
    if (TryGetComponent(out Player player))
    {
        player.Die(); // ✅ play animation, delay destruction
    }
}


}
