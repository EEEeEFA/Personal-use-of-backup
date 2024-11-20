using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    Inventory inventory = Inventory.instance;
    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
        
        if(currentHP <= GetMaxHealthValue() * .1f)
        {
            inventory.GetEquipedEquipment(EquipmentType.Armor);
        }
    }

    protected override void Start()
    {
        base.Start();
    }
}
