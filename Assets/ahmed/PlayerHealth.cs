using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : CharacterStats
{
    public static PlayerHealth instance;

    public Slider slider;

    protected override void Start()
    {
        base.Start();
        instance = this;

        if (slider)
        {
            slider.maxValue = GetMaxHealthValue();
            slider.value = currentHp;
        }

        onHealhtChanged += UpdateSlider;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); // Handles currentHp reduction and death

        UpdateSlider();
    }

    private void UpdateSlider()
    {
        if (slider)
        {
            slider.maxValue = GetMaxHealthValue();
            slider.value = currentHp;
        }
    }

    public bool IsDead()
    {
        return currentHp <= 0;
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Player Died!");
        // Add death animation or respawn here
    }

    private void OnDisable()
    {
        onHealhtChanged -= UpdateSlider;
    }
}
