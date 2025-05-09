using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public Stat damage;
    public Stat maxHealth;



    [SerializeField] private int currentHp;



    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHealth.GetValue();
    }

    public void TakeDamage(int _damage)
    {
        currentHp -= _damage;


        if (currentHp < 0)
        {
            Die();
        }
    }

    private static void Die()
    {

    }
}
