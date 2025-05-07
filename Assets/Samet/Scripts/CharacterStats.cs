using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public int damage;
    public int maxHp;

    [SerializeField] private int currentHp;



    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
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
