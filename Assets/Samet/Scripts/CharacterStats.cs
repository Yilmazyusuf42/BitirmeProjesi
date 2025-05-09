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
    
    public virtual void DoDamage(CharacterStats _targerStats)
    {

        Debug.Log("Uygulandý");

        int totalDamage = damage.GetValue()+strength.GetValue();

        _targerStats.TakeDamage(totalDamage);

    }

    public virtual void TakeDamage(int _damage)
    {
        currentHp -= _damage;

        Debug.Log(_damage); 


        if (currentHp < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

    }
}
