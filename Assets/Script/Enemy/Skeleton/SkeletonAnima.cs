using UnityEngine;
using static Entity;

public class SkeletonAnima : MonoBehaviour
{
    E_Skeleton enemy => GetComponentInParent<E_Skeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationTriggerCalled();
    }

    private void AttackTrigger()//�����ж�
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(enemy.setAttackP.position, enemy.attackCheckRadius);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Player>() != null)
            {
                Player _player = hit.GetComponent<Player>();
                _player.DamageEffect(enemy);//�ܻ�λ�ơ���Ч

                CharacterStats _target = hit.GetComponent<CharacterStats>();
                enemy.stats.DoDamage(_target, _player);//�ܻ��˺�����
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
