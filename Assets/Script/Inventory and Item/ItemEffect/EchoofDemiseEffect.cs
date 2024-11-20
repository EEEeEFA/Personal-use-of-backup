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
    //TODO 回忆血条做法
    //血量低于10%
    //在DecreaseHealthBy(int _damage)里触发 yep Intellegence

    //技能冷却，放在inventory里

    //减速周围敌人90% 2秒
}
