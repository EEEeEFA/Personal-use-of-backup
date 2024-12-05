using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect ", menuName = "Data/Item Effect/HealingEffect")]
public class HealingEffect : itemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;
    public override void UseEffect(Transform _enemyTarget)
    {
        PlayerStats _playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount = Mathf.RoundToInt(healPercent * _playerStats.GetMaxHealthValue());//治疗数值取整

        _playerStats.IncreaseHealthBy(healAmount);
    }
}
