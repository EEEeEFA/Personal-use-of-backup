using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect ", menuName = "Data/Item Effect/Echo of Demise")]
public class EchoofDemiseEffect : itemEffect
{
    public override void UseEffect(Transform _enemyTarget)
    {
        base.UseEffect(_enemyTarget);
    }
    //TODO ����Ѫ������
    //Ѫ������10%
    //��DecreaseHealthBy(int _damage)�ﴥ�� yep Intellegence

    //������ȴ������inventory��

    //������Χ����90% 2��
}
