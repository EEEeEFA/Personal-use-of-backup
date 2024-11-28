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

    private void AttackTrigger()//攻击判定
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(player.setAttackP.position, player.attackCheckRadius);//第一周作业 画圆检测 

        // Physics.SphereCast (position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy _enemy = hit.GetComponent<Enemy>();
                EnemyStats _enemyStats = hit.GetComponent<EnemyStats>();


                _enemy.DamageEffect(player);//击退效果 和 视觉效果

                player.stats.DoDamage(_enemyStats, _enemy);//伤害计算

                DynamicEquipmentEffect(_enemy.transform);//装备效果

            }
        }
    }

    private void DynamicEquipmentEffect(Transform _enemyTarget)//主动型装备效果 普攻、防反触发
    {
        
        ItemData_Equipment equipedItem = Inventory.instance.GetEquipedEquipment(EquipmentType.Weapon);
        if (equipedItem != null)
        {
                equipedItem.UseDynamicItemEffect(_enemyTarget);
        }
    }

    private void CounterAttackTrigger()//反击判定
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(player.setAttackP.position, player.attackCheckRadius);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy _enemy = hit.GetComponent<Enemy>();

                if (_enemy.StunCheck())//判断敌人是否在stunWindow内
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
    private void PassvityEquipmentEffect(Transform _enemyTarget)//被动性装备效果 只有防反触发
    {
        ItemData_Equipment equipedItem = Inventory.instance.GetEquipedEquipment(EquipmentType.Weapon);
        if (equipedItem != null)
        {
            equipedItem.UsePassiveItemEffect(_enemyTarget);
        }
    }

    private void CreateSword()//动画处调用
    {
        player.skillManager.TS.CreateSword();
    }

}
