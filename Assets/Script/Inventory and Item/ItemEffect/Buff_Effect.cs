using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightingDamage
}



[CreateAssetMenu(fileName = "New Item Effect ", menuName = "Data/Item Effect/BuffEffect")]
public class Buff_Effect : itemEffect
{
    public StatType BuffType;
    [SerializeField]private int buffValue;//�ӵ���ֵ
    [SerializeField] private int buffDuration;//����ʱ��

    private PlayerStats stats;

    private Dictionary<StatType, Func<Stats>> statLookup;

    private void OnEnable()
    {
        statLookup = new Dictionary<StatType, Func<Stats>>
        {
            { StatType.strength, () => stats.strength },
            { StatType.agility, () => stats.agility },
            { StatType.intelligence, () => stats.intelligence },
            { StatType.vitality, () => stats.vitality },
            { StatType.damage, () => stats.dealDamage },
            { StatType.critChance, () => stats.critChance },
            { StatType.critPower, () => stats.critPower }, 
            { StatType.health, () => stats.maxHP },
            { StatType.armor, () => stats.armor },
            { StatType.evasion, () => stats.evasion },
            { StatType.magicRes, () => stats.magicResistance },
            { StatType.fireDamage, () => stats.fireDamage },
            { StatType.iceDamage, () => stats.iceDamage },
            { StatType.lightingDamage, () => stats.lightningDamage }
        };
    }

    private Stats StatToModify()
    {
         if(statLookup.TryGetValue(BuffType, out var statFunc))
        {
            return statFunc(); // ���� Lambda ���ʽ����������ֵ
        }

        throw new ArgumentException($"û���Stat��: {BuffType}");
    }
    

public override void UseEffect(Transform _enemyTarget)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffValue, buffDuration, StatToModify());
    }
}
