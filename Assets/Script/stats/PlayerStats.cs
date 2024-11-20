using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
        
        if(currentHP <= GetMaxHealthValue() * .1f)//Ѫ������
        {
            ItemData_Equipment _Armor = Inventory.instance.GetEquipedEquipment(EquipmentType.Armor);//����Ƿ���װ��

            if (_Armor)
            {
                if (!_Armor.CoolDownCounter())//��ȴʱ�����
                        return;

                     Debug.Log("UseEffect");
                    _Armor.UsePassiveItemEffect(player.transform);//TODO ������PlayerManager.instance.player.transform���޷�����
            }

        }
    }

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }
}
