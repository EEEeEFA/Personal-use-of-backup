using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    private PlayerSkillManager skillManager;
    void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;

        skillManager = PlayerSkillManager.instance;

    }
    void Update()
    {
        CheckCoolDown();
    }
    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHP;
    }

    private void SetCoolDownOfDash()
    {

    }
    private void CheckCoolDown()
    {

    }
}
