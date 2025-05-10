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

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critDamage; // default value is %150

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;






    [SerializeField] private int currentHp;



    // Start is called before the first frame update
   protected virtual void Start()
    {
        critDamage.SetDefaultValue(150);
        currentHp = maxHealth.GetValue();
    }
    
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }
        int totalDamage = damage.GetValue() + strength.GetValue();
        if (CanCrit())
        {
           totalDamage=CalculateCriticalDamage(totalDamage);
            Debug.Log(totalDamage);
        }
      
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
       // _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);
    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignite,bool _chill,bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;
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

    }
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    private bool CanCrit()
    {
        int totalCritchance = critChance.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) <= totalCritchance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritDamage= (critDamage.GetValue()+strength.GetValue())*.01f;
        Debug.Log("Total crit " + totalCritDamage);
        float critDamge = _damage * totalCritDamage;

        return Mathf.RoundToInt(critDamge);
    }
}
