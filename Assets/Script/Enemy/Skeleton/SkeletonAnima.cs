using UnityEngine;
using static Entity;

public class SkeletonAnima : MonoBehaviour
{
    E_Skeleton enemy => GetComponentInParent<E_Skeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationTriggerCalled();
    }

    private void AttackTrigger()//攻击判定
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(enemy.setAttackP.position, enemy.attackCheckRadius);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Player>() != null)
            {
                Player _player = hit.GetComponent<Player>();
                _player.DamageEffect(enemy);//受击位移、特效

                CharacterStats _target = hit.GetComponent<CharacterStats>();
                enemy.stats.DoDamage(_target, _player);//受击伤害计算
            }
        }
    }

    private void StunWindowClose() => enemy.StunWindowClose();
    private void StunWindowOpen()=> enemy.StunWindowOpen();

    private void BoomAndDrop()
    {
        enemy.dropSystem.GenerateDrop();
        Destroy(transform.parent.gameObject);
    }
}
