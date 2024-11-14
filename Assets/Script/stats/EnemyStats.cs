using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [Header("Level details")]
    [SerializeField] private int level;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier;

    ItemDrop dropSystem => GetComponent<ItemDrop>();
    protected override void Start()
    {
        ApplyUpgrade();

        base.Start();
    }

    private void ApplyUpgrade()
    {
        UpLevelModifier(strength);
        UpLevelModifier(agility);
        UpLevelModifier(intelligence);
        UpLevelModifier(vitality);

        UpLevelModifier(dealDamage);
        UpLevelModifier(critChance);
        UpLevelModifier(critPower);

        UpLevelModifier(maxHP);
        UpLevelModifier(armor);
        UpLevelModifier(evasion);
        //UpLevelModifier(magicResistance);

        //UpLevelModifier(fireDamage);
        //UpLevelModifier(iceDamage);
        //UpLevelModifier(lightningDamage);
    }

    public void UpLevelModifier(Stats _stats)
    {
        for( int i = 1; i < level; i++)
        {
            float Upgrade = _stats.GetValue() * percentageModifier;

            _stats.AddModifier(Mathf.RoundToInt(Upgrade));
        }
    }

    protected override void Die(Entity _beAttacked)
    {
        base.Die(_beAttacked);
        dropSystem.DropItem();//ÎïÆ·µôÂä
    }
}
