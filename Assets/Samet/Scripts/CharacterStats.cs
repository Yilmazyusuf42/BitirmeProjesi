using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat strength;
    public Stat damage;
    public Stat maxHealth;



    [SerializeField] private int currentHp;



    // Start is called before the first frame update
   protected virtual void Start()
    {
        currentHp = maxHealth.GetValue();
    }
    
public virtual void DoDamage(CharacterStats targetStats)
{
    int totalDamage = damage.GetValue() + strength.GetValue();
    Debug.Log($"[DoDamage] Dealing {totalDamage} damage");
    targetStats.TakeDamage(totalDamage);
}


public virtual void TakeDamage(int damage)
{
    currentHp -= damage;

    Debug.Log($"{gameObject.name} took {damage} damage. Current HP: {currentHp}");

    if (currentHp <= 0)
        Die();
}


protected virtual void Die()
{
    if (TryGetComponent(out EnemyBase enemy))
    {
        enemy.Die(); // ✅ call enemy death logic
    }
    else
    {
        Debug.LogWarning($"{name} died but has no EnemyBase script.");
        Destroy(gameObject);
    }
}

}
