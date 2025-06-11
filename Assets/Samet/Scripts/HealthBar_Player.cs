using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Player : MonoBehaviour
{
    private PlayerStats playerStats;
    private Slider slider;

    private void Awake()
    {
        // Automatically find the slider on this GameObject or children
        slider = GetComponentInChildren<Slider>();

        if (slider == null)
            Debug.LogError("❌ HealthBar_Player: Slider not found!");
    }

    private void Start()
    {
        // Find PlayerStats once the game starts
        playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats == null)
        {
            Debug.LogError("❌ HealthBar_Player: PlayerStats not found in scene!");
            return;
        }

        // Set initial values
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHp;

        // Subscribe to health change
        playerStats.onHealhtChanged += UpdateHealthUI;
    }

    private void UpdateHealthUI()
    {
        if (slider != null && playerStats != null)
        {
            slider.maxValue = playerStats.GetMaxHealthValue();
            slider.value = playerStats.currentHp;
        }
    }

    private void OnDisable()
    {
        if (playerStats != null)
            playerStats.onHealhtChanged -= UpdateHealthUI;
    }
}
