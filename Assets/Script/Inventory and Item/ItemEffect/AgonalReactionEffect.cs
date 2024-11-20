using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect ", menuName = "Data/Item Effect/Agonalreaction")]
public class AllFreeze : itemEffect
{
    [SerializeField] float CircleRadius;
    [SerializeField] float TimeToFreeze;
    public override void UseEffect(Transform _transform)//传玩家坐标
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(_transform.position, CircleRadius);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 100);//寻找并冻结敌人

        foreach (var hit in colliders)//敌人冻结逻辑
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Debug.Log("冻结");
                hit.GetComponent<Enemy>()?.FreezeTimeFor(TimeToFreeze);//?是为了保证有这个组件
            }

        }

            //    Enemy _enemy = hit.GetComponent<Enemy>();
            //if (_enemy != null)
            //{
            //    _enemy.FreezeTimeFor(TimeToFreeze);
            //    Debug.Log(_enemy.name);
            //}


    }
    //血量低于10%
    //在DecreaseHealthBy(int _damage)里触发

    //技能冷却，模块放在itemData_Equipment里 模块在DecreaseHealthBy调用

    //冻结周围敌人 for TimeToFreeze
}
