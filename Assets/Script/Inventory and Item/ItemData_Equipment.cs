using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    public itemEffect[] dynamicEffects;
    public itemEffect[] passvieEffects;

    //装备冷却时间
    [Header("Equipment Counter")]
    static float lastTimeUsed = -Mathf.Infinity;
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


    public void AddModifiers()
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

    public void RemoveModifiers()
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
        //playerStats.magicResistance.RemoveModifier(magicResistance);

        //playerStats.fireDamage.RemoveModifier(fireDamage);
        //playerStats.iceDamage.RemoveModifier(iceDamage);
        //playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }

    public void UseDynamicItemEffect(Transform _Target)
    {
        for (int i = 0;i < dynamicEffects.Length; i++)
        {
            dynamicEffects[i].UseEffect(_Target);
        }
    }

    public void UsePassiveItemEffect(Transform _Target)//使用装备 passvieEffects栏 中的效果
    {
        for (int i = 0; i < passvieEffects.Length; i++)
        {
            passvieEffects[i].UseEffect(_Target);
        }
    }

    public bool CoolDownCounter()
    {
        //Debug.Log(Time.time);
        //Debug.Log(CoolDownTime);
        //Debug.Log(lastTimeUsed);
        if (Time.time > CoolDownTime + lastTimeUsed)
        {
            lastTimeUsed = Time.time;
            return true;
        }
        else 
            return false;
    }
}
