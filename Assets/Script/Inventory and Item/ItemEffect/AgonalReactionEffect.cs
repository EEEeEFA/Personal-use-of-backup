using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect ", menuName = "Data/Item Effect/Agonalreaction")]
public class AllFreeze : itemEffect
{
    [SerializeField] float CircleRadius;
    [SerializeField] float TimeToFreeze;
    public override void UseEffect(Transform _transform)//���������
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(_transform.position, CircleRadius);

        foreach (var hit in collider)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(TimeToFreeze);

    
            //    Enemy _enemy = hit.GetComponent<Enemy>();
            //if (_enemy != null)
            //{
            //    _enemy.FreezeTimeFor(TimeToFreeze);
            //    Debug.Log(_enemy.name);
            //}
        }

    }
    //Ѫ������10%
    //��DecreaseHealthBy(int _damage)�ﴥ��

    //������ȴ��ģ�����itemData_Equipment�� ģ����DecreaseHealthBy����

    //������Χ���� for TimeToFreeze
}
