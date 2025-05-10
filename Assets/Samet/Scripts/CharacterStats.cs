using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{


    [Header("Major Stats")]
    public Stat strength; //1 point increase damage by 1 and crit power by %1
    public Stat agility;  // 1 point increase evasion by %1 and crit chance by %1
    public Stat intelligence; // 1 point increase magick damage by 1 and magic resistance by 3
    public Stat vitality; // 1 point increase health by 3 or 5 points


    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;


    public Stat damage;



    [SerializeField] private int currentHp;



    // Start is called before the first frame update
   protected virtual void Start()
    {
        currentHp = maxHealth.GetValue();
    }
    
    public virtual void DoDamage(CharacterStats _targerStats)
    {
        int totalEvasion= _targerStats.evasion.GetValue()+_targerStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Attack Avoided");
            return;
        }

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
