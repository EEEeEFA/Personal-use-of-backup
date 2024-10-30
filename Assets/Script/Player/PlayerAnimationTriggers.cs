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

    private void AttackTrigger()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(player.setAttackP.position, player.attackCheckRadius);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage(player);
        }
    }

    private void CounterAttackTrigger()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(player.setAttackP.position, player.attackCheckRadius);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
                if (hit.GetComponent<Enemy>().StunCheck())
                {
                    Debug.Log("Stunned!!");
                    AnimationTrigger();

                }


        }
    }

    private void CreateSword()
    {
        player.skill.TS.CreateSword();
    }

}
