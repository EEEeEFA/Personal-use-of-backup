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
                hit.GetComponent<Enemy>().Damage(player);
                hit.GetComponent<CharacterStats>().TakeDamage(player.stats.dealDamage.GetVuale());
        }
    }

    private void CounterAttackTrigger()//反击判定
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(player.setAttackP.position, player.attackCheckRadius);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
                if (hit.GetComponent<Enemy>().StunCheck())//判断敌人是否在stunWindow内
                {
                    AnimationTrigger();

                }


        }
    }

    private void CreateSword()
    {
        player.skill.TS.CreateSword();
    }

}
