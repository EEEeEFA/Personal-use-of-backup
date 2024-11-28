using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Entity;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()//�����ж�
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(player.setAttackP.position, player.attackCheckRadius);//��һ����ҵ ��Բ��� 

        // Physics.SphereCast (position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy _enemy = hit.GetComponent<Enemy>();
                EnemyStats _enemyStats = hit.GetComponent<EnemyStats>();


                _enemy.DamageEffect(player);//����Ч�� �� �Ӿ�Ч��

                player.stats.DoDamage(_enemyStats, _enemy);//�˺�����

                DynamicEquipmentEffect(_enemy.transform);//װ��Ч��

            }
        }
    }

    private void DynamicEquipmentEffect(Transform _enemyTarget)//������װ��Ч�� �չ�����������
    {
        
        ItemData_Equipment equipedItem = Inventory.instance.GetEquipedEquipment(EquipmentType.Weapon);
        if (equipedItem != null)
        {
                equipedItem.UseDynamicItemEffect(_enemyTarget);
        }
    }

    private void CounterAttackTrigger()//�����ж�
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(player.setAttackP.position, player.attackCheckRadius);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy _enemy = hit.GetComponent<Enemy>();

                if (_enemy.StunCheck())//�жϵ����Ƿ���stunWindow��
                {
                    AnimationTrigger();

                    CharacterStats _target = hit.GetComponent<CharacterStats>();

                    player.stats.DoDamage(_target, _enemy);

                    PassvityEquipmentEffect(_enemy.transform);
                    DynamicEquipmentEffect(_enemy.transform);
                }

            }


        }
    }
    private void PassvityEquipmentEffect(Transform _enemyTarget)//������װ��Ч�� ֻ�з�������
    {
        ItemData_Equipment equipedItem = Inventory.instance.GetEquipedEquipment(EquipmentType.Weapon);
        if (equipedItem != null)
        {
            equipedItem.UsePassiveItemEffect(_enemyTarget);
        }
    }

    private void CreateSword()//����������
    {
        player.skillManager.TS.CreateSword();
    }

}
