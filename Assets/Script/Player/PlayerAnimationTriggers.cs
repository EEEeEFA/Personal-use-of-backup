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
                hit.GetComponent<Enemy>().Damage(player);

                EnemyStats _target = hit.GetComponent<EnemyStats>();

                player.stats.DoDamage(_target);

                
            }
        }
    }

    private void CounterAttackTrigger()//�����ж�
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(player.setAttackP.position, player.attackCheckRadius);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
                if (hit.GetComponent<Enemy>().StunCheck())//�жϵ����Ƿ���stunWindow��
                {
                    AnimationTrigger();
                    hit.GetComponent<CharacterStats>().TakeDamage(player.stats.dealDamage.GetValue());
                    Debug.Log("�����˺�="+player.stats.dealDamage.GetValue());
                }


        }
    }

    private void CreateSword()
    {
        player.skill.TS.CreateSword();
    }

}
