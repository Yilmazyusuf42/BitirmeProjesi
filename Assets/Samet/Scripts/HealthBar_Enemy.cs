using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Enemy : MonoBehaviour
{
    private EnemyBase _enemyBase;
    private CharacterStats myStats;
    private RectTransform _rectTransform;
    private Slider slider;
    private void Start()
    {
        _enemyBase = GetComponentInParent<EnemyBase>();
        _rectTransform = GetComponent<RectTransform>();
        slider=GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        _enemyBase.onFlipped += FlipUI;
        myStats.onHealhtChanged += UpdateHealthUI;

        UpdateHealthUI();
    }
    private void Update()
    {
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHp;
    }
    private void FlipUI() => _rectTransform.Rotate(0, 180, 0);
    private void OnDisable()
    {
        _enemyBase.onFlipped -= FlipUI;
        myStats.onHealhtChanged-= UpdateHealthUI;
    }
}
