using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 热更新装备基类，所有热更新装备都应该继承此类
/// </summary>
public abstract class HotUpdateEquipmentBase : ItemData, IEquipment
{
    [Header("Equipment Properties")]
    public EquipmentType equipmentType;
    public itemEffect[] dynamicEffects;
    public itemEffect[] passiveEffects;
    
    [Header("Equipment Counter")]
    protected static float lastTimeUsed = -Mathf.Infinity;
    [SerializeField] public float CoolDownTime;

    [Header("Major stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive stats")]
    public int dealDamage;
    public int critChance;
    public int critPower;

    [Header("Defensive stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    [Header("Craft Material")]
    public List<InventoryItem> craftingMaterials;

    // 接口实现
    public virtual string GetEquipmentName() => itemName;
    public virtual Sprite GetEquipmentIcon() => icon;
    public virtual string GetEquipmentId() => itemId;
    public virtual EquipmentType GetEquipmentType() => equipmentType;
    public virtual float GetCoolDownTime() => CoolDownTime;

    public virtual void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.dealDamage.AddModifier(dealDamage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHP.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
    }

    public virtual void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.dealDamage.RemoveModifier(dealDamage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHP.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }

    public virtual void UseDynamicItemEffect(Transform _Target)
    {
        if (dynamicEffects != null)
        {
            for (int i = 0; i < dynamicEffects.Length; i++)
            {
                if (dynamicEffects[i] != null)
                    dynamicEffects[i].UseEffect(_Target);
            }
        }
    }

    public virtual void UsePassiveItemEffect(Transform _Target)
    {
        if (passiveEffects != null)
        {
            for (int i = 0; i < passiveEffects.Length; i++)
            {
                if (passiveEffects[i] != null)
                    passiveEffects[i].UseEffect(_Target);
            }
        }
    }

    public virtual bool CoolDownCounter()
    {
        if (Time.time > CoolDownTime + lastTimeUsed)
        {
            lastTimeUsed = Time.time;
            return true;
        }
        else 
            return false;
    }

    // 抽象方法，子类必须实现
    public abstract void OnEquip();
    public abstract void OnUnequip();
    public abstract void OnUse();
} 