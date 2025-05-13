using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Player : MonoBehaviour
{
    private CharacterStats myStats;
    private Entity _entity;
    private RectTransform _rectTransform;
    private Slider slider;
    private void Start()
    {
        _entity = GetComponentInParent<Entity>();
        _rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();


        _entity.onFlipped += FlipUI;
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
        _entity.onFlipped -= FlipUI;
        myStats.onHealhtChanged -= UpdateHealthUI;
    }
}
