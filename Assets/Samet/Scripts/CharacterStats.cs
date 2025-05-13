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

    public bool isIgnited; //does damage over time
    public bool isChilled; // reduce armor %20
    public bool isShocked; // reduce accuracy %20


    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float igniteDamageCooldown=.3f;
    private float ignitedDamgeTimer;
    private int igniteDamage;




    public int currentHp;

    public System.Action onHealhtChanged;

    // Start is called before the first frame update
   protected virtual void Start()
    {
        critDamage.SetDefaultValue(150);
        currentHp = GetMaxHealthValue();
    }
    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        ignitedDamgeTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;
        if(chilledTimer<0)
            isChilled = false;
        if(shockedTimer<0)
            isShocked = false;


        if (ignitedDamgeTimer < 0 &&isIgnited)
        {
        
            Debug.Log("TAKE not burn DAMAGE");

            DecreaseHealthBy(igniteDamage);

            if(currentHp < 0)
            {
                Die();
            }
            
            ignitedDamgeTimer = igniteDamageCooldown;
        }
    }
    
    public virtual void DoDamage(CharacterStats _targetStats,bool isPhysicalDamage)
    {
        Debug.Log(isPhysicalDamage);
        if (isPhysicalDamage)
        {
            if (TargetCanAvoidAttack(_targetStats))
            {
                return;
            }
            int totalDamage = damage.GetValue() + strength.GetValue();
            if (CanCrit())
            {
                totalDamage = CalculateCriticalDamage(totalDamage);
              //  Debug.Log(totalDamage);
            }

            totalDamage = CheckTargetArmor(_targetStats, totalDamage);
             _targetStats.TakeDamage(totalDamage);
        }
        else
        {
            DoMagicalDamage(_targetStats);
        }  
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
            return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _iceDamage && _lightningDamage > _fireDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if(Random.value<.5f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite,canApplyChill,canApplyShock);
                return;
            }
            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;

            }
            if (Random.value < .5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
        {
            float applyIgniteDamage = _fireDamage * .1f;
            if (applyIgniteDamage > 0 && applyIgniteDamage < 1)
                applyIgniteDamage = 1;
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .1f));
        }
           

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
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

        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = 4;
        }
        if (_chill)
        {
            isChilled = _chill;
            chilledTimer = 4;
        }
        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = 4;
        }
    }
    public void SetupIgniteDamage(int _damage)=>igniteDamage = _damage;
    public virtual void TakeDamage(int damage)
{

    currentHp -= damage;

   // Debug.Log($"{gameObject.name} took {damage} damage. Current HP: {currentHp}");

    if (currentHp <= 0)
        Die();
}
protected virtual void DecreaseHealthBy(int _damage)
{
    currentHp -= _damage;

    if (onHealhtChanged != null)
        onHealhtChanged();
}


    protected virtual void Die()
    {

    }
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage = Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
        {
            int damageReductionRate = (_targetStats.armor.GetValue() / totalDamage);

            damageReductionRate = Mathf.Clamp(damageReductionRate, 0, 75);
            totalDamage/=damageReductionRate;

            Debug.Log(damageReductionRate);
        }

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion -= 20;
        }

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

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue();
    }
}
