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

                EquipmentEffect(_enemy.transform);//װ��Ч��

            }
        }
    }

    private static void EquipmentEffect(Transform _enemyTarget)
    {
        List<ItemData_Equipment> equipedItem = Inventory.instance.GetEquipedEquipment(EquipmentType.Weapon);
        if (equipedItem != null)
        {

            for (int i = 0; i < equipedItem.Count; i++)//�Լ�д�ĳ���˫��Ƕ��ʺɽ�����һ���Ժ��ٸ�
            {
                equipedItem[i].UseItemEffect(_enemyTarget);
            }
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
                }


            }


        }
    }

    private void CreateSword()//����������
    {
        player.skill.TS.CreateSword();
    }

}
