using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    RectTransform uiTransform;
    CharacterStats myStats;
    Entity entity;
    Slider slider;
    private void Start()
    {
        uiTransform = GetComponent<RectTransform>();
        myStats = GetComponentInParent<CharacterStats>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponent<Slider>();

        entity.onFilpped += FlipUI;
        myStats.onHealthChanged += UpdateSlider;

        UpdateSlider();//重开时更新血量
    }
    
    private void UpdateSlider()//更新血条
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHP;
    }

    private void FlipUI()=> uiTransform.Rotate(0,180,0);

    private void OnDisable()
    {
        entity.onFilpped -= FlipUI;
        myStats.onHealthChanged -= UpdateSlider;
    }
}
