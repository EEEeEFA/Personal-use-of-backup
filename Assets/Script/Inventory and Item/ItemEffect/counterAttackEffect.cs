using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect ", menuName = "Data/Item Effect/counterAttackEffect")]
public class counterAttackEffect : itemEffect
{
    [SerializeField]GameObject barbPrefabs;
    public override void UseEffect(Transform _enemyTarget)
    {
        Transform _player = PlayerManager.instance.player.transform;
        GameObject newbarbPrefabs = Instantiate(barbPrefabs, _player.position, _player.rotation);
        Destroy(newbarbPrefabs, .7f);//TRY 在动画结束的时候删除

    }
}
