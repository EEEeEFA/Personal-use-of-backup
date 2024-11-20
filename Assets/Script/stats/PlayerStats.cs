using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
        
        if(currentHP <= GetMaxHealthValue() * .1f)//血量鉴定
        {
            ItemData_Equipment _Armor = Inventory.instance.GetEquipedEquipment(EquipmentType.Armor);//检测是否穿了装备

            if (_Armor)
            {
                if (!_Armor.CoolDownCounter())//冷却时间鉴定
                        return;

                     Debug.Log("UseEffect");
                    _Armor.UsePassiveItemEffect(player.transform);//TODO 这里用PlayerManager.instance.player.transform会无法运行
            }

        }
    }

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }
}
