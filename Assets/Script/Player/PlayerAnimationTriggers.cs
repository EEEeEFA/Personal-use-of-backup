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
                _enemy.DamageEffect(player);

                EnemyStats _target = hit.GetComponent<EnemyStats>();

                player.stats.DoDamage(_target, _enemy);

                
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
