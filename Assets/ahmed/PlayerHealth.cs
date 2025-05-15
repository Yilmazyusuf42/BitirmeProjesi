using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider slider;
    private bool isDead = false; // Track death state

    void Start()
    {
        instance = this;
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Don’t take damage if already dead

        currentHealth -= damage;
        slider.value = currentHealth;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player Died!");
        // Add death animation or logic here if needed
    }

    // Public method to check if player is dead
    public bool IsDead()
    {
        return isDead;
    }
}