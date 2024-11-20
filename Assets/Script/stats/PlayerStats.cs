using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    Inventory inventory = Inventory.instance;
    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
        
        if(currentHP <= GetMaxHealthValue() * .1f)//血量鉴定
        {
            if (!Inventory.instance.CanUseArmor())//冷却时间鉴定
                return;
            ItemData_Equipment _Armor = inventory.GetEquipedEquipment(EquipmentType.Armor);
            if (_Armor != null)
            {   
              
                _Armor.UsePassiveItemEffect(null);
            }
        }
    }

    protected override void Start()
    {
        base.Start();
    }
}
