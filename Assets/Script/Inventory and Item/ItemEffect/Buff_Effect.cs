using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Effect ", menuName = "Data/Item Effect/BuffEffect")]
public class Buff_Effect : itemEffect
{
    public StatType BuffType;
    [SerializeField]private int buffValue;//�ӵ���ֵ
    [SerializeField] private int buffDuration;//����ʱ��

    private PlayerStats stats;
public override void UseEffect(Transform _enemyTarget)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffValue, buffDuration, stats.GetStat(BuffType));
    }
}
