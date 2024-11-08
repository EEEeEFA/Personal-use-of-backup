using UnityEngine;
using static Entity;

public class SkeletonAnima : MonoBehaviour
{
    E_Skeleton enemy => GetComponentInParent<E_Skeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationTriggerCalled();
    }

    private void AttackTrigger()//¹¥»÷ÅÐ¶¨
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(enemy.setAttackP.position, enemy.attackCheckRadius);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Player>() != null)
            {
                //hit.GetComponent<Player>().Damage(enemy);
                CharacterStats _target = hit.GetComponent<CharacterStats>();
                enemy.stats.DoDamage(_target);
            }
        }
    }

    private void StunWindowClose() => enemy.StunWindowClose();
    private void StunWindowOpen()=> enemy.StunWindowOpen();
}
